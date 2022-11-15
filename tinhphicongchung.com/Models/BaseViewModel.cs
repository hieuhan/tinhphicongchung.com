using System;
using System.Collections.Generic;
using System.Web;
using tinhphicongchung.com.helper;
using tinhphicongchung.com.library;

namespace tinhphicongchung.com.Models
{
    public class BaseViewModel
    {
        private string _websiteTitle;
        private string _metaTitle;
        private string _metaDescription;
        private string _metaKeywords;
        private string _websiteImage;
        private string _seoHeader;
        private string _seoFooter;
        private string _websiteCanonical;

        public string WebsiteTitle
        {
            get
            {
                if (!string.IsNullOrEmpty(_websiteTitle))
                {
                    _websiteTitle = _websiteTitle.Replace("\"", "");
                }
                else
                {
                    _websiteTitle = ConstantHelper.WebsiteTitle;
                }

                return _websiteTitle;
            }
            set { _websiteTitle = value; }
        }

        public string MetaTitle
        {
            get
            {
                if (!string.IsNullOrEmpty(_metaTitle))
                {
                    _metaTitle = _metaTitle.Replace(Environment.NewLine, " ");
                    if (!_metaTitle.Contains("content="))
                        _metaTitle = string.Concat("content=\"", _metaTitle, "\"");
                }
                else _metaTitle = ConstantHelper.WebsiteTitle;

                return _metaTitle;
            }
            set { _metaTitle = value; }
        }

        public string MetaDescription
        {
            get
            {
                if (!string.IsNullOrEmpty(_metaDescription))
                {
                    if (!_metaDescription.Contains("content="))
                        _metaDescription = string.Concat("content=\"", _metaDescription.Replace("\"", ""), "\"");
                }
                else _metaDescription = ConstantHelper.WebsiteDescription;

                return _metaDescription;
            }
            set
            {
                _metaDescription = value;
            }
        }

        public string MetaKeywords
        {
            get
            {
                if (!string.IsNullOrEmpty(_metaKeywords))
                {
                    if (!_metaKeywords.Contains("content="))
                        _metaKeywords = string.Concat("content=\"", _metaKeywords.Replace("\"", ""), "\"");
                }
                else _metaKeywords = ConstantHelper.WebsiteKeywords;

                return _metaKeywords;
            }
            set
            {
                _metaKeywords = value;
            }
        }

        public string WebsiteCanonical
        {
            get
            {
                if (string.IsNullOrEmpty(_websiteCanonical))
                {
                    _websiteCanonical = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
                }

                return _websiteCanonical;
            }
            set { _websiteCanonical = value; }
        }

        public string WebsiteImage
        {
            get
            {
                if (string.IsNullOrEmpty(_websiteImage))
                    _websiteImage = ConstantHelper.NoImageUrl;
                return _websiteImage;
            }
            set { _websiteImage = value; }
        }

        public string SeoName { get; set; }

        public string SeoHeader
        {
            get
            {
                _seoHeader = !string.IsNullOrEmpty(_seoHeader) ? _seoHeader.GetTitle() : ConstantHelper.DefaultSeoHeader;
                return _seoHeader;
            }
            set { _seoHeader = value; }
        }

        public string SeoFooter
        {
            get
            {
                _seoFooter = !string.IsNullOrEmpty(_seoFooter) ? _seoFooter.Replace("\"", "") : string.Empty;
                return _seoFooter;
            }
            set { _seoFooter = value; }
        }

        public string SeoDescription { get; set; }

        public int VisitorOnline { get; set; }  
        public HitCounter HitCounter { get; set; }
        public Pages Pages { get; set; }
        public List<MenuItems> MenuItemsList { get; set; }
        public Seos Seos { get; set; }
    }
}