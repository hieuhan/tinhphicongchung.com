/* Bengin mySidebar*/
jQuery(document).ready(function($) {
var pushbar = new Pushbar({
    blur:true,
    overlay:true,
  });
 });
class Pushbar {
  constructor(config = { overlay: true, blur: false }) {
    this.activeBar = null;
    this.overlay = false;

    if (config.overlay) {
      this.overlay = document.createElement('div');
      this.overlay.classList.add('pushbar_overlay');
      document.querySelector('body').appendChild(this.overlay);
    }

    if (config.blur) {
      const mainContent = document.querySelector('.pushbar_main_content');
      if (mainContent) {
        mainContent.classList.add('pushbar_blur');
      }
    }

    this.bindEvents();
  }

  get opened() {
    const { activeBar } = this;
    return Boolean(activeBar instanceof HTMLElement && activeBar.classList.contains('opened'));
  }
  
  get activeBarId() {
    const { activeBar } = this;
    return activeBar instanceof HTMLElement && activeBar.getAttribute('data-pushbar-id');
  }

  static dispatchOpen(pushbar) {
    const event = new CustomEvent('pushbar_opening', { bubbles: true, detail: { pushbar } });
    pushbar.dispatchEvent(event);
  }

  static dispatchClose(pushbar) {
    const event = new CustomEvent('pushbar_closing', { bubbles: true, detail: { pushbar } });
    pushbar.dispatchEvent(event);
  }

  static findElementById(pushbarId) {
    return document.querySelector(`[data-pushbar-id="${pushbarId}"]`);
  }

  handleOpenEvent(e) {
    e.preventDefault();
    const pushbarId = e.currentTarget.getAttribute('data-pushbar-target');
    if (pushbarId) {
      this.open(pushbarId);
    }
  }

  handleCloseEvent(e) {
    e.preventDefault();
    this.close();
  }

  handleKeyEvent(e) {
    if (this.opened && e.keyCode === 27) {
      this.close();
    }
  }

  bindEvents() {
    const triggers = document.querySelectorAll('[data-pushbar-target]');
    const closers = document.querySelectorAll('[data-pushbar-close]');

    triggers.forEach(trigger => trigger.addEventListener('click', e => this.handleOpenEvent(e), false));
    closers.forEach(closer => closer.addEventListener('click', e => this.handleCloseEvent(e), false));

    if (this.overlay) {
      this.overlay.addEventListener('click', e => this.handleCloseEvent(e), false);
    }
    document.addEventListener('keyup', e => this.handleKeyEvent(e));
  }

  open(pushbarId) {
    // Current bar is already opened
    if (String(pushbarId) === this.activeBarId && this.opened) {
      return;
    }
    
    // Get new pushbar target
    const pushbar = Pushbar.findElementById(pushbarId);

    if (!pushbar) return;
    
    // Close active bar (if exists)
    if (this.opened) {
      this.close();
    }
    
    Pushbar.dispatchOpen(pushbar);
    pushbar.classList.add('opened');

    const Root = document.querySelector('body');
    Root.classList.add('pushbar_locked');
    Root.setAttribute('pushbar', pushbarId);
    this.activeBar = pushbar;
  }

  close() {
    const { activeBar } = this;
    if (!activeBar) return;
    
    Pushbar.dispatchClose(activeBar);
    activeBar.classList.remove('opened');

    const Root = document.querySelector('body');
    Root.classList.remove('pushbar_locked');
    Root.removeAttribute('pushbar');
    
    this.activeBar = null;
  }
}
/* End mySidebar*/

/* Bengin sticky_nav */
jQuery(function($) {
// Create a clone of the menu, right next to original.
$('.sticky_nav').addClass('original_sticky').clone().insertAfter('.sticky_nav').addClass('cloned_sticky').css('position','fixed').css('top','0').css('margin-top','0').css('z-index','999').removeClass('original_sticky').hide();
scrollIntervalID = setInterval(stickIt, 10);
function stickIt() {
  if($('.sticky_nav').length){
    var orgElementPos = $('.original_sticky').offset();
  orgElementTop = orgElementPos.top;               
  if ($(window).scrollTop() >= (orgElementTop)) {
    // scrolled past the original position; now only show the cloned, sticky element.
    // Cloned element should always have same left position and width as original element.     
    orgElement = $('.original_sticky');
    coordsOrgElement = orgElement.offset();
    leftOrgElement = coordsOrgElement.left;  
    widthOrgElement = orgElement.css('width');
    $('.cloned_sticky').css('left',leftOrgElement+'px').css('top',0).css('width',widthOrgElement).show();
    $('.original_sticky').css('visibility','hidden');
  } else {
    // not scrolled past the menu; only show the original menu.
    $('.cloned_sticky').hide();
    $('.original_sticky').css('visibility','visible');
  }
  }
}
});
/* End sticky_nav */