using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using tinhphicongchung.com.helper;
using tinhphicongchung.com.library;
using tinhphicongchung.com.Models;

namespace tinhphicongchung.com.Controllers
{
    public class NotarizationFeeController : Controller
    {
        readonly CacheHelper cacheHelper = new CacheHelper();

        // GET: NotarizationFee

        #region LoanContract

        /// <summary>
        /// Hợp đồng vay tiền
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> LoanContract()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);

                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result,
                };
            }

            LoanContractVM model = new LoanContractVM
            {
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList
            };

            if (cacheVM.SeosList.IsAny())
            {
                model.Seos = cacheVM.SeosList.FirstOrDefault(x => Request.Url.AbsolutePath.GetUrl().Equals(x.Path.GetUrl()));

                if (model.Seos == null || model.Seos.SeoId <= 0 || string.IsNullOrWhiteSpace(model.Seos.Path))
                {
                    return Redirect(Url.Action("NotFound", "Home"));
                }

                if (!Request.Url.AbsolutePath.Contains(model.Seos.Path))
                {
                    return RedirectPermanent(model.Seos.Path.GetUrl());
                }

                model.WebsiteTitle = model.Seos.MetaTitle;
                model.MetaTitle = model.Seos.MetaTitle;
                model.MetaDescription = model.Seos.MetaDescription;
                model.MetaKeywords = model.Seos.MetaKeyword;
                model.SeoHeader = model.Seos.H1Tag;

                if (!string.IsNullOrWhiteSpace(model.Seos.Name))
                    model.SeoName = model.Seos.Name;

                if (!string.IsNullOrWhiteSpace(model.Seos.Description))
                    model.SeoDescription = model.Seos.Description;

                if (!string.IsNullOrWhiteSpace(model.Seos.SeoFooter))
                    model.SeoFooter = model.Seos.SeoFooter;

                if (!string.IsNullOrWhiteSpace(model.Seos.ImagePath))
                    model.WebsiteImage = model.Seos.ImagePath.GetImageUrl();
            }

            return View(model);
        }

        /// <summary>
        /// Hợp đồng vay tiền
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> LoanContract(LoanContractVM model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(model.SubmitAction))
                {
                    model.Amount = model.Amount.Replace(",", "").Replace(".", "");
                    Contracts contracts = await Contracts.Static_LoanContract(double.Parse(model.Amount));

                    if (contracts != null && !string.IsNullOrWhiteSpace(contracts.ActionStatus) && contracts.ActionStatus.Equals(ConstantHelper.ActionStatusSuccess))
                    {
                        return Json(new
                        {
                            Completed = true,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('.row-ketqua').find('.phi-cong-chung').first().html('{0:0,0}');$('.row-ketqua').removeClass('hidden');if($(window).scrollTop() > 150) $(\"html, body\").animate({{ scrollTop: $('.section-shadow').offset().top }});", contracts.NotarizationFee)))
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            Completed = false,
                            Message = contracts.ActionMessage
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new
                    {
                        Completed = true,
                        Data = "NoAction",
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new
            {
                Completed = false,
                Message = "Quý khách vui lòng thử lại sau."
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ContractAmendmentsAndSupplements

        /// <summary>
        /// Hợp đồng sửa đổi, bổ sung có thay đổi giá trị tài sản
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ContractAmendmentsAndSupplements()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);

                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result
                };
            }

            ContractAmendmentsAndSupplementsVM model = new ContractAmendmentsAndSupplementsVM
            {
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList
            };

            if (cacheVM.SeosList.IsAny())
            {
                model.Seos = cacheVM.SeosList.FirstOrDefault(x => Request.Url.AbsolutePath.GetUrl().Equals(x.Path.GetUrl()));

                if (model.Seos == null || model.Seos.SeoId <= 0 || string.IsNullOrWhiteSpace(model.Seos.Path))
                {
                    return Redirect(Url.Action("NotFound", "Home"));
                }

                if (!Request.Url.AbsolutePath.Contains(model.Seos.Path))
                {
                    return RedirectPermanent(model.Seos.Path.GetUrl());
                }

                model.WebsiteTitle = model.Seos.MetaTitle;
                model.MetaTitle = model.Seos.MetaTitle;
                model.MetaDescription = model.Seos.MetaDescription;
                model.MetaKeywords = model.Seos.MetaKeyword;
                model.SeoHeader = model.Seos.H1Tag;

                if (!string.IsNullOrWhiteSpace(model.Seos.Name))
                    model.SeoName = model.Seos.Name;

                if (!string.IsNullOrWhiteSpace(model.Seos.Description))
                    model.SeoDescription = model.Seos.Description;

                if (!string.IsNullOrWhiteSpace(model.Seos.SeoFooter))
                    model.SeoFooter = model.Seos.SeoFooter;

                if (!string.IsNullOrWhiteSpace(model.Seos.ImagePath))
                    model.WebsiteImage = model.Seos.ImagePath.GetImageUrl();
            }

            return View(model);
        }

        /// <summary>
        /// Hợp đồng sửa đổi, bổ sung có thay đổi giá trị tài sản
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ContractAmendmentsAndSupplements(ContractAmendmentsAndSupplementsVM model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(model.SubmitAction))
                {
                    model.Amount = model.Amount.Replace(",", "").Replace(".", "");
                    Contracts contracts = await Contracts.Static_ContractAmendmentsAndSupplements(double.Parse(model.Amount));

                    if (contracts != null && !string.IsNullOrWhiteSpace(contracts.ActionStatus) && contracts.ActionStatus.Equals(ConstantHelper.ActionStatusSuccess))
                    {
                        return Json(new
                        {
                            Completed = true,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('.row-ketqua').find('.phi-cong-chung').first().html('{0:0,0}');$('.row-ketqua').removeClass('hidden');if($(window).scrollTop() > 150) $(\"html, body\").animate({{ scrollTop: $('.section-shadow').offset().top }});", contracts.NotarizationFee)))
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            Completed = false,
                            Message = contracts.ActionMessage
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new
                    {
                        Completed = true,
                        Data = "NoAction",
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new
            {
                Completed = false,
                Message = "Quý khách vui lòng thử lại sau."
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region EconomicCommercialInvestmentBusinessContract

        /// <summary>
        /// Hợp đồng kinh tế, thương mại, đầu tư, kinh doanh
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> EconomicCommercialInvestmentBusinessContract()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);

                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result
                };
            }

            EconomicCommercialInvestmentBusinessContractVM model = new EconomicCommercialInvestmentBusinessContractVM
            {
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList
            };

            if (cacheVM.SeosList.IsAny())
            {
                model.Seos = cacheVM.SeosList.FirstOrDefault(x => Request.Url.AbsolutePath.GetUrl().Equals(x.Path.GetUrl()));

                if (model.Seos == null || model.Seos.SeoId <= 0 || string.IsNullOrWhiteSpace(model.Seos.Path))
                {
                    return Redirect(Url.Action("NotFound", "Home"));
                }

                if (!Request.Url.AbsolutePath.Contains(model.Seos.Path))
                {
                    return RedirectPermanent(model.Seos.Path.GetUrl());
                }

                model.WebsiteTitle = model.Seos.MetaTitle;
                model.MetaTitle = model.Seos.MetaTitle;
                model.MetaDescription = model.Seos.MetaDescription;
                model.MetaKeywords = model.Seos.MetaKeyword;
                model.SeoHeader = model.Seos.H1Tag;

                if (!string.IsNullOrWhiteSpace(model.Seos.Name))
                    model.SeoName = model.Seos.Name;

                if (!string.IsNullOrWhiteSpace(model.Seos.Description))
                    model.SeoDescription = model.Seos.Description;

                if (!string.IsNullOrWhiteSpace(model.Seos.SeoFooter))
                    model.SeoFooter = model.Seos.SeoFooter;

                if (!string.IsNullOrWhiteSpace(model.Seos.ImagePath))
                    model.WebsiteImage = model.Seos.ImagePath.GetImageUrl();
            }

            return View(model);
        }

        /// <summary>
        /// Hợp đồng kinh tế, thương mại, đầu tư, kinh doanh
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> EconomicCommercialInvestmentBusinessContract(EconomicCommercialInvestmentBusinessContractVM model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(model.SubmitAction))
                {
                    model.Amount = model.Amount.Replace(",", "").Replace(".", "");
                    Contracts contracts = await Contracts.Static_EconomicCommercialInvestmentBusinessContract(double.Parse(model.Amount));

                    if (contracts != null && !string.IsNullOrWhiteSpace(contracts.ActionStatus) && contracts.ActionStatus.Equals(ConstantHelper.ActionStatusSuccess))
                    {
                        return Json(new
                        {
                            Completed = true,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('.row-ketqua').find('.phi-cong-chung').first().html('{0:0,0}');$('.row-ketqua').removeClass('hidden');if($(window).scrollTop() > 150) $(\"html, body\").animate({{ scrollTop: $('.section-shadow').offset().top }});", contracts.NotarizationFee)))
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            Completed = false,
                            Message = contracts.ActionMessage
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new
                    {
                        Completed = true,
                        Data = "NoAction",
                    }, JsonRequestBehavior.AllowGet);
                }  
            }

            return Json(new
            {
                Completed = false,
                Message = "Quý khách vui lòng thử lại sau."
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region HouseLeaseContract

        /// <summary>
        /// Hợp đồng thuê Nhà
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> HouseLeaseContract()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);

                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result
                };
            }

            HouseLeaseContractVM model = new HouseLeaseContractVM
            {
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList
            };

            if (cacheVM.SeosList.IsAny())
            {
                model.Seos = cacheVM.SeosList.FirstOrDefault(x => Request.Url.AbsolutePath.GetUrl().Equals(x.Path.GetUrl()));

                if (model.Seos == null || model.Seos.SeoId <= 0 || string.IsNullOrWhiteSpace(model.Seos.Path))
                {
                    return Redirect(Url.Action("NotFound", "Home"));
                }

                if (!Request.Url.AbsolutePath.Contains(model.Seos.Path))
                {
                    return RedirectPermanent(model.Seos.Path.GetUrl());
                }

                model.WebsiteTitle = model.Seos.MetaTitle;
                model.MetaTitle = model.Seos.MetaTitle;
                model.MetaDescription = model.Seos.MetaDescription;
                model.MetaKeywords = model.Seos.MetaKeyword;
                model.SeoHeader = model.Seos.H1Tag;

                if (!string.IsNullOrWhiteSpace(model.Seos.Name))
                    model.SeoName = model.Seos.Name;

                if (!string.IsNullOrWhiteSpace(model.Seos.Description))
                    model.SeoDescription = model.Seos.Description;

                if (!string.IsNullOrWhiteSpace(model.Seos.SeoFooter))
                    model.SeoFooter = model.Seos.SeoFooter;

                if (!string.IsNullOrWhiteSpace(model.Seos.ImagePath))
                    model.WebsiteImage = model.Seos.ImagePath.GetImageUrl();
            }

            return View(model);
        }

        /// <summary>
        /// Hợp đồng thuê Nhà
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> HouseLeaseContract(HouseLeaseContractVM model)
        {
            if (ModelState.IsValid)
            {
                model.Amount = model.Amount.Replace(",", "").Replace(".", "");
                Contracts contracts = await Contracts.Static_HouseLeaseContract(int.Parse(model.RentalPeriod), double.Parse(model.Amount));

                if (contracts != null && !string.IsNullOrWhiteSpace(contracts.ActionStatus) && contracts.ActionStatus.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('.row-ketqua').find('.phi-cong-chung').first().html('{0:0,0}');$('.row-ketqua').removeClass('hidden');if($(window).scrollTop() > 150) $(\"html, body\").animate({{ scrollTop: $('.section-shadow').offset().top }});", contracts.NotarizationFee)))
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Completed = false,
                        Message = contracts.ActionMessage
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new
            {
                Completed = false,
                Message = "Quý khách vui lòng thử lại sau."
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region MortgageContract

        /// <summary>
        /// Hợp đồng thế chấp
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> MortgageContract()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);
                
                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result
                };
            }

            MortgageContractVM model = new MortgageContractVM
            {
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList
            };

            if (cacheVM.SeosList.IsAny())
            {
                model.Seos = cacheVM.SeosList.FirstOrDefault(x => Request.Url.AbsolutePath.GetUrl().Equals(x.Path.GetUrl()));

                if (model.Seos == null || model.Seos.SeoId <= 0 || string.IsNullOrWhiteSpace(model.Seos.Path))
                {
                    return Redirect(Url.Action("NotFound", "Home"));
                }

                if (!Request.Url.AbsolutePath.Contains(model.Seos.Path))
                {
                    return RedirectPermanent(model.Seos.Path.GetUrl());
                }

                model.WebsiteTitle = model.Seos.MetaTitle;
                model.MetaTitle = model.Seos.MetaTitle;
                model.MetaDescription = model.Seos.MetaDescription;
                model.MetaKeywords = model.Seos.MetaKeyword;
                model.SeoHeader = model.Seos.H1Tag;

                if (!string.IsNullOrWhiteSpace(model.Seos.Name))
                    model.SeoName = model.Seos.Name;

                if (!string.IsNullOrWhiteSpace(model.Seos.Description))
                    model.SeoDescription = model.Seos.Description;

                if (!string.IsNullOrWhiteSpace(model.Seos.SeoFooter))
                    model.SeoFooter = model.Seos.SeoFooter;

                if (!string.IsNullOrWhiteSpace(model.Seos.ImagePath))
                    model.WebsiteImage = model.Seos.ImagePath.GetImageUrl();
            }

            return View(model);
        }

        /// <summary>
        /// Hợp đồng thế chấp
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> MortgageContract(MortgageContractVM model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(model.SubmitAction))
                {
                    model.Amount = model.Amount.Replace(",", "").Replace(".", "");
                    Contracts contracts = await Contracts.Static_MortgageContract(double.Parse(model.Amount));

                    if (contracts != null && !string.IsNullOrWhiteSpace(contracts.ActionStatus) && contracts.ActionStatus.Equals(ConstantHelper.ActionStatusSuccess))
                    {
                        return Json(new
                        {
                            Completed = true,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('.row-ketqua').find('.phi-cong-chung').first().html('{0:0,0}');$('.row-ketqua').removeClass('hidden');if($(window).scrollTop() > 150) $(\"html, body\").animate({{ scrollTop: $('.section-shadow').offset().top }});", contracts.NotarizationFee)))
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            Completed = false,
                            Message = contracts.ActionMessage
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new
                    {
                        Completed = true,
                        Data = "NoAction",
                    }, JsonRequestBehavior.AllowGet);
                }    
            }

            return Json(new
            {
                Completed = false,
                Message = "Quý khách vui lòng thử lại sau."
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region DepositContract

        /// <summary>
        /// Hợp đồng đặt cọc
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> DepositContract()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);
               
                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result
                };
            }

            DepositContractVM model = new DepositContractVM
            {
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList
            };

            if (cacheVM.SeosList.IsAny())
            {
                model.Seos = cacheVM.SeosList.FirstOrDefault(x => Request.Url.AbsolutePath.GetUrl().Equals(x.Path.GetUrl()));

                if (model.Seos == null || model.Seos.SeoId <= 0 || string.IsNullOrWhiteSpace(model.Seos.Path))
                {
                    return Redirect(Url.Action("NotFound", "Home"));
                }

                if (!Request.Url.AbsolutePath.Contains(model.Seos.Path))
                {
                    return RedirectPermanent(model.Seos.Path.GetUrl());
                }

                model.WebsiteTitle = model.Seos.MetaTitle;
                model.MetaTitle = model.Seos.MetaTitle;
                model.MetaDescription = model.Seos.MetaDescription;
                model.MetaKeywords = model.Seos.MetaKeyword;
                model.SeoHeader = model.Seos.H1Tag;

                if (!string.IsNullOrWhiteSpace(model.Seos.Name))
                    model.SeoName = model.Seos.Name;

                if (!string.IsNullOrWhiteSpace(model.Seos.Description))
                    model.SeoDescription = model.Seos.Description;

                if (!string.IsNullOrWhiteSpace(model.Seos.SeoFooter))
                    model.SeoFooter = model.Seos.SeoFooter;

                if (!string.IsNullOrWhiteSpace(model.Seos.ImagePath))
                    model.WebsiteImage = model.Seos.ImagePath.GetImageUrl();
            }

            return View(model);
        }

        /// <summary>
        /// Hợp đồng đặt cọc
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DepositContract(DepositContractVM model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(model.SubmitAction))
                {
                    model.Amount = model.Amount.Replace(",", "").Replace(".", "");
                    Contracts contracts = await Contracts.Static_DepositContract(double.Parse(model.Amount));

                    if (contracts != null && !string.IsNullOrWhiteSpace(contracts.ActionStatus) && contracts.ActionStatus.Equals(ConstantHelper.ActionStatusSuccess))
                    {
                        return Json(new
                        {
                            Completed = true,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('.row-ketqua').find('.phi-cong-chung').first().html('{0:0,0}');$('.row-ketqua').removeClass('hidden');if($(window).scrollTop() > 150) $(\"html, body\").animate({{ scrollTop: $('.section-shadow').offset().top }});", contracts.NotarizationFee)))
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            Completed = false,
                            Message = contracts.ActionMessage
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new
                    {
                        Completed = true,
                        Data = "NoAction",
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new
            {
                Completed = false,
                Message = "Quý khách vui lòng thử lại sau."
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region CapitalContributionContract

        /// <summary>
        /// Hợp đồng góp vốn bằng tiền
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> CapitalContributionContract()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);
                
                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result
                };
            }

            CapitalContributionContractVM model = new CapitalContributionContractVM
            {
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList
            };

            if (cacheVM.SeosList.IsAny())
            {
                model.Seos = cacheVM.SeosList.FirstOrDefault(x => Request.Url.AbsolutePath.GetUrl().Equals(x.Path.GetUrl()));

                if (model.Seos == null || model.Seos.SeoId <= 0 || string.IsNullOrWhiteSpace(model.Seos.Path))
                {
                    return Redirect(Url.Action("NotFound", "Home"));
                }

                if (!Request.Url.AbsolutePath.Contains(model.Seos.Path))
                {
                    return RedirectPermanent(model.Seos.Path.GetUrl());
                }

                model.WebsiteTitle = model.Seos.MetaTitle;
                model.MetaTitle = model.Seos.MetaTitle;
                model.MetaDescription = model.Seos.MetaDescription;
                model.MetaKeywords = model.Seos.MetaKeyword;
                model.SeoHeader = model.Seos.H1Tag;

                if (!string.IsNullOrWhiteSpace(model.Seos.Name))
                    model.SeoName = model.Seos.Name;

                if (!string.IsNullOrWhiteSpace(model.Seos.Description))
                    model.SeoDescription = model.Seos.Description;

                if (!string.IsNullOrWhiteSpace(model.Seos.SeoFooter))
                    model.SeoFooter = model.Seos.SeoFooter;

                if (!string.IsNullOrWhiteSpace(model.Seos.ImagePath))
                    model.WebsiteImage = model.Seos.ImagePath.GetImageUrl();
            }

            return View(model);
        }

        /// <summary>
        /// Hợp đồng góp vốn bằng tiền
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> CapitalContributionContract(CapitalContributionContractVM model)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrWhiteSpace(model.SubmitAction))
                {
                    model.Amount = model.Amount.Replace(",", "").Replace(".", "");
                    Contracts contracts = await Contracts.Static_CapitalContributionContract(double.Parse(model.Amount));

                    if (contracts != null && !string.IsNullOrWhiteSpace(contracts.ActionStatus) && contracts.ActionStatus.Equals(ConstantHelper.ActionStatusSuccess))
                    {
                        return Json(new
                        {
                            Completed = true,
                            Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('.row-ketqua').find('.phi-cong-chung').first().html('{0:0,0}');$('.row-ketqua').removeClass('hidden');if($(window).scrollTop() > 150) $(\"html, body\").animate({{ scrollTop: $('.section-shadow').offset().top }});", contracts.NotarizationFee)))
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new
                        {
                            Completed = false,
                            Message = contracts.ActionMessage
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new
                    {
                        Completed = true,
                        Data = "NoAction",
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new
            {
                Completed = false,
                Message = "Quý khách vui lòng thử lại sau."
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ContractCapitalContributionByApartment

        /// <summary>
        /// Hợp đồng góp vốn bằng căn hộ Chung Cư
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ContractCapitalContributionByApartment()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<ApartmentTypes>> apartmentTypesTask = ApartmentTypes.Static_GetList();
                Task<List<YearBuilts>> yearBuiltsTask = YearBuilts.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Apartments>> apartmentsTask = Apartments.Static_GetList(ConstantHelper.StatusIdActivated);

                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask, apartmentTypesTask, yearBuiltsTask, apartmentsTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result,
                    ApartmentTypesList = apartmentTypesTask.Result,
                    ApartmentsList = apartmentsTask.Result,
                    YearBuiltsList = yearBuiltsTask.Result
                };
            }

            ContractCapitalContributionByApartmentVM model = new ContractCapitalContributionByApartmentVM
            {
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList,
                ApartmentTypesList = cacheVM.ApartmentTypesList.IsAny() ? cacheVM.ApartmentTypesList : new List<ApartmentTypes>(),
                YearBuiltsList = cacheVM.YearBuiltsList.IsAny() ? cacheVM.YearBuiltsList : new List<YearBuilts>(),
                ApartmentsList = cacheVM.ApartmentsList.IsAny() ? cacheVM.ApartmentsList : new List<Apartments>()
            };

            if (cacheVM.SeosList.IsAny())
            {
                model.Seos = cacheVM.SeosList.FirstOrDefault(x => Request.Url.AbsolutePath.GetUrl().Equals(x.Path.GetUrl()));

                if (model.Seos == null || model.Seos.SeoId <= 0 || string.IsNullOrWhiteSpace(model.Seos.Path))
                {
                    return Redirect(Url.Action("NotFound", "Home"));
                }

                if (!Request.Url.AbsolutePath.Contains(model.Seos.Path))
                {
                    return RedirectPermanent(model.Seos.Path.GetUrl());
                }

                model.WebsiteTitle = model.Seos.MetaTitle;
                model.MetaTitle = model.Seos.MetaTitle;
                model.MetaDescription = model.Seos.MetaDescription;
                model.MetaKeywords = model.Seos.MetaKeyword;
                model.SeoHeader = model.Seos.H1Tag;

                if (!string.IsNullOrWhiteSpace(model.Seos.Name))
                    model.SeoName = model.Seos.Name;

                if (!string.IsNullOrWhiteSpace(model.Seos.Description))
                    model.SeoDescription = model.Seos.Description;

                if (!string.IsNullOrWhiteSpace(model.Seos.SeoFooter))
                    model.SeoFooter = model.Seos.SeoFooter;

                if (!string.IsNullOrWhiteSpace(model.Seos.ImagePath))
                    model.WebsiteImage = model.Seos.ImagePath.GetImageUrl();
            }

            return View(model);
        }

        /// <summary>
        /// Hợp đồng góp vốn bằng căn hộ Chung Cư
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ContractCapitalContributionByApartment(ContractCapitalContributionByApartmentVM model)
        {
            if (string.IsNullOrWhiteSpace(model.HouseArea))
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích Nhà"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.HouseArea.Trim().Length > 9)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích Nhà tối đa 9 ký tự"
                }, JsonRequestBehavior.AllowGet);
            }

            var houseAreaMatch = Regex.Match(model.HouseArea, @"^[0-9]+\.?([0-9]+)?$", RegexOptions.IgnoreCase);

            if (!houseAreaMatch.Success)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích Nhà hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.ApartmentTypeId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "ApartmentTypeId",
                    Message = "Vui lòng chọn Loại Nhà"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.Year <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "Year",
                    Message = "Vui lòng chọn Năm xây dựng"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.ApartmentId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "ApartmentId",
                    Message = "Vui lòng chọn Cấp công trình"
                }, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrWhiteSpace(model.SubmitAction))
            {
                model.HouseArea = model.HouseArea.Replace(",", ".");

                Contracts contracts = await Contracts.Static_ContractCapitalContributionByApartment(double.Parse(model.HouseArea, System.Globalization.CultureInfo.InvariantCulture), model.ApartmentTypeId, model.Year, model.ApartmentId);

                if (contracts != null && !string.IsNullOrWhiteSpace(contracts.ActionStatus) && contracts.ActionStatus.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('.row-ketqua').find('.gia-tri-nha-theo-khung-gia-nha-nuoc').first().html('{0:0,0}');$('.row-ketqua').find('.phi-cong-chung').first().html('{1:0,0}');$('.row-ketqua').removeClass('hidden');if($(window).scrollTop() > 150) $(\"html, body\").animate({{ scrollTop: $('.section-shadow').offset().top }});", contracts.AssetValue, contracts.NotarizationFee)))
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Completed = false,
                        Message = contracts.ActionMessage
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    Completed = true,
                    Data = "NoAction",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region PurchaseAndSaleApartmentContract

        /// <summary>
        /// Hợp đồng mua bán căn hộ Chung Cư
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> PurchaseAndSaleApartmentContract()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<ApartmentTypes>> apartmentTypesTask = ApartmentTypes.Static_GetList();
                Task<List<YearBuilts>> yearBuiltsTask = YearBuilts.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Apartments>> apartmentsTask = Apartments.Static_GetList(ConstantHelper.StatusIdActivated);

                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask, apartmentTypesTask, yearBuiltsTask, apartmentsTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result,
                    ApartmentTypesList = apartmentTypesTask.Result,
                    ApartmentsList = apartmentsTask.Result,
                    YearBuiltsList = yearBuiltsTask.Result
                };
            }

            PurchaseAndSaleApartmentContractVM model = new PurchaseAndSaleApartmentContractVM
            {
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList,
                ApartmentTypesList = cacheVM.ApartmentTypesList.IsAny() ? cacheVM.ApartmentTypesList : new List<ApartmentTypes>(),
                YearBuiltsList = cacheVM.YearBuiltsList.IsAny() ? cacheVM.YearBuiltsList : new List<YearBuilts>(),
                ApartmentsList = cacheVM.ApartmentsList.IsAny() ? cacheVM.ApartmentsList : new List<Apartments>()
            };

            if (cacheVM.SeosList.IsAny())
            {
                model.Seos = cacheVM.SeosList.FirstOrDefault(x => Request.Url.AbsolutePath.GetUrl().Equals(x.Path.GetUrl()));

                if (model.Seos == null || model.Seos.SeoId <= 0 || string.IsNullOrWhiteSpace(model.Seos.Path))
                {
                    return Redirect(Url.Action("NotFound", "Home"));
                }

                if (!Request.Url.AbsolutePath.Contains(model.Seos.Path))
                {
                    return RedirectPermanent(model.Seos.Path.GetUrl());
                }

                model.WebsiteTitle = model.Seos.MetaTitle;
                model.MetaTitle = model.Seos.MetaTitle;
                model.MetaDescription = model.Seos.MetaDescription;
                model.MetaKeywords = model.Seos.MetaKeyword;
                model.SeoHeader = model.Seos.H1Tag;

                if (!string.IsNullOrWhiteSpace(model.Seos.Name))
                    model.SeoName = model.Seos.Name;

                if (!string.IsNullOrWhiteSpace(model.Seos.Description))
                    model.SeoDescription = model.Seos.Description;

                if (!string.IsNullOrWhiteSpace(model.Seos.SeoFooter))
                    model.SeoFooter = model.Seos.SeoFooter;

                if (!string.IsNullOrWhiteSpace(model.Seos.ImagePath))
                    model.WebsiteImage = model.Seos.ImagePath.GetImageUrl();
            }

            return View(model);
        }

        /// <summary>
        /// Hợp đồng mua bán căn hộ Chung Cư
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> PurchaseAndSaleApartmentContract(PurchaseAndSaleApartmentContractVM model)
        {
            if (string.IsNullOrWhiteSpace(model.HouseArea))
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích Nhà"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.HouseArea.Trim().Length > 9)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích Nhà tối đa 9 ký tự"
                }, JsonRequestBehavior.AllowGet);
            }

            var houseAreaMatch = Regex.Match(model.HouseArea, @"^[0-9]+\.?([0-9]+)?$", RegexOptions.IgnoreCase);

            if (!houseAreaMatch.Success)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích Nhà hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.ApartmentTypeId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "ApartmentTypeId",
                    Message = "Vui lòng chọn Loại Nhà"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.Year <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "Year",
                    Message = "Vui lòng chọn Năm xây dựng"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.ApartmentId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "ApartmentId",
                    Message = "Vui lòng chọn Cấp công trình"
                }, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrWhiteSpace(model.SubmitAction))
            {
                model.HouseArea = model.HouseArea.Replace(",", ".");

                Contracts contracts = await Contracts.Static_PurchaseAndSaleApartmentContract(double.Parse(model.HouseArea, System.Globalization.CultureInfo.InvariantCulture), model.ApartmentTypeId, model.Year, model.ApartmentId);

                if (contracts != null && !string.IsNullOrWhiteSpace(contracts.ActionStatus) && contracts.ActionStatus.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('.row-ketqua').find('.gia-tri-nha-theo-khung-gia-nha-nuoc').first().html('{0:0,0}');$('.row-ketqua').find('.phi-cong-chung').first().html('{1:0,0}');$('.row-ketqua').removeClass('hidden');if($(window).scrollTop() > 150) $(\"html, body\").animate({{ scrollTop: $('.section-shadow').offset().top }});", contracts.AssetValue, contracts.NotarizationFee)))
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Completed = false,
                        Message = contracts.ActionMessage
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    Completed = true,
                    Data = "NoAction",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region ContractDonationApartment

        /// <summary>
        /// Hợp đồng tặng cho căn hộ Chung Cư
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ContractDonationApartment()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<ApartmentTypes>> apartmentTypesTask = ApartmentTypes.Static_GetList();
                Task<List<YearBuilts>> yearBuiltsTask = YearBuilts.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Apartments>> apartmentsTask = Apartments.Static_GetList(ConstantHelper.StatusIdActivated);

                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask, apartmentTypesTask, yearBuiltsTask, apartmentsTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result,
                    ApartmentTypesList = apartmentTypesTask.Result,
                    ApartmentsList = apartmentsTask.Result,
                    YearBuiltsList = yearBuiltsTask.Result
                };
            }

            ContractDonationApartmentVM model = new ContractDonationApartmentVM
            {
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList,
                ApartmentTypesList = cacheVM.ApartmentTypesList.IsAny() ? cacheVM.ApartmentTypesList : new List<ApartmentTypes>(),
                YearBuiltsList = cacheVM.YearBuiltsList.IsAny() ? cacheVM.YearBuiltsList : new List<YearBuilts>(),
                ApartmentsList = cacheVM.ApartmentsList.IsAny() ? cacheVM.ApartmentsList : new List<Apartments>()
            };

            if (cacheVM.SeosList.IsAny())
            {
                model.Seos = cacheVM.SeosList.FirstOrDefault(x => Request.Url.AbsolutePath.GetUrl().Equals(x.Path.GetUrl()));

                if (model.Seos == null || model.Seos.SeoId <= 0 || string.IsNullOrWhiteSpace(model.Seos.Path))
                {
                    return Redirect(Url.Action("NotFound", "Home"));
                }

                if (!Request.Url.AbsolutePath.Contains(model.Seos.Path))
                {
                    return RedirectPermanent(model.Seos.Path.GetUrl());
                }

                model.WebsiteTitle = model.Seos.MetaTitle;
                model.MetaTitle = model.Seos.MetaTitle;
                model.MetaDescription = model.Seos.MetaDescription;
                model.MetaKeywords = model.Seos.MetaKeyword;
                model.SeoHeader = model.Seos.H1Tag;

                if (!string.IsNullOrWhiteSpace(model.Seos.Name))
                    model.SeoName = model.Seos.Name;

                if (!string.IsNullOrWhiteSpace(model.Seos.Description))
                    model.SeoDescription = model.Seos.Description;

                if (!string.IsNullOrWhiteSpace(model.Seos.SeoFooter))
                    model.SeoFooter = model.Seos.SeoFooter;

                if (!string.IsNullOrWhiteSpace(model.Seos.ImagePath))
                    model.WebsiteImage = model.Seos.ImagePath.GetImageUrl();
            }


            return View(model);
        }

        /// <summary>
        /// Hợp đồng tặng cho căn hộ Chung Cư
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ContractDonationApartment(ContractDonationApartmentVM model)
        {
            if (string.IsNullOrWhiteSpace(model.HouseArea))
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích Nhà"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.HouseArea.Trim().Length > 9)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích Nhà tối đa 9 ký tự"
                }, JsonRequestBehavior.AllowGet);
            }

            var houseAreaMatch = Regex.Match(model.HouseArea, @"^[0-9]+\.?([0-9]+)?$", RegexOptions.IgnoreCase);

            if (!houseAreaMatch.Success)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích Nhà hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.ApartmentTypeId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "ApartmentTypeId",
                    Message = "Vui lòng chọn Loại Nhà"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.Year <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "Year",
                    Message = "Vui lòng chọn Năm xây dựng"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.ApartmentId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "ApartmentId",
                    Message = "Vui lòng chọn Cấp công trình"
                }, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrWhiteSpace(model.SubmitAction))
            {
                model.HouseArea = model.HouseArea.Replace(",", ".");

                Contracts contracts = await Contracts.Static_ContractDonationApartment(double.Parse(model.HouseArea, System.Globalization.CultureInfo.InvariantCulture), model.ApartmentTypeId, model.Year, model.ApartmentId);

                if (contracts != null && !string.IsNullOrWhiteSpace(contracts.ActionStatus) && contracts.ActionStatus.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('.row-ketqua').find('.gia-tri-nha-theo-khung-gia-nha-nuoc').first().html('{0:0,0}');$('.row-ketqua').find('.phi-cong-chung').first().html('{1:0,0}');$('.row-ketqua').removeClass('hidden');if($(window).scrollTop() > 150) $(\"html, body\").animate({{ scrollTop: $('.section-shadow').offset().top }});", contracts.AssetValue, contracts.NotarizationFee)))
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Completed = false,
                        Message = contracts.ActionMessage
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    Completed = true,
                    Data = "NoAction",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region DocumentDivideInheritanceApartment

        /// <summary>
        /// Văn bản chia di sản thừa kế là căn hộ Chung Cư
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> DocumentDivideInheritanceApartment()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<ApartmentTypes>> apartmentTypesTask = ApartmentTypes.Static_GetList();
                Task<List<YearBuilts>> yearBuiltsTask = YearBuilts.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Apartments>> apartmentsTask = Apartments.Static_GetList(ConstantHelper.StatusIdActivated);

                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask, apartmentTypesTask, yearBuiltsTask, apartmentsTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result,
                    ApartmentTypesList = apartmentTypesTask.Result,
                    ApartmentsList = apartmentsTask.Result,
                    YearBuiltsList = yearBuiltsTask.Result
                };
            }

            DocumentDivideInheritanceApartmentVM model = new DocumentDivideInheritanceApartmentVM
            {
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList,
                ApartmentTypesList = cacheVM.ApartmentTypesList.IsAny() ? cacheVM.ApartmentTypesList : new List<ApartmentTypes>(),
                YearBuiltsList = cacheVM.YearBuiltsList.IsAny() ? cacheVM.YearBuiltsList : new List<YearBuilts>(),
                ApartmentsList = cacheVM.ApartmentsList.IsAny() ? cacheVM.ApartmentsList : new List<Apartments>()
            };

            if (cacheVM.SeosList.IsAny())
            {
                model.Seos = cacheVM.SeosList.FirstOrDefault(x => Request.Url.AbsolutePath.GetUrl().Equals(x.Path.GetUrl()));

                if (model.Seos == null || model.Seos.SeoId <= 0 || string.IsNullOrWhiteSpace(model.Seos.Path))
                {
                    return Redirect(Url.Action("NotFound", "Home"));
                }

                if (!Request.Url.AbsolutePath.Contains(model.Seos.Path))
                {
                    return RedirectPermanent(model.Seos.Path.GetUrl());
                }

                model.WebsiteTitle = model.Seos.MetaTitle;
                model.MetaTitle = model.Seos.MetaTitle;
                model.MetaDescription = model.Seos.MetaDescription;
                model.MetaKeywords = model.Seos.MetaKeyword;
                model.SeoHeader = model.Seos.H1Tag;

                if (!string.IsNullOrWhiteSpace(model.Seos.Name))
                    model.SeoName = model.Seos.Name;

                if (!string.IsNullOrWhiteSpace(model.Seos.Description))
                    model.SeoDescription = model.Seos.Description;

                if (!string.IsNullOrWhiteSpace(model.Seos.SeoFooter))
                    model.SeoFooter = model.Seos.SeoFooter;

                if (!string.IsNullOrWhiteSpace(model.Seos.ImagePath))
                    model.WebsiteImage = model.Seos.ImagePath.GetImageUrl();
            }

            return View(model);
        }

        /// <summary>
        /// Văn bản chia di sản thừa kế là căn hộ Chung Cư
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DocumentDivideInheritanceApartment(DocumentDivideInheritanceApartmentVM model)
        {
            if (string.IsNullOrWhiteSpace(model.HouseArea))
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích Nhà"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.HouseArea.Trim().Length > 9)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích Nhà tối đa 9 ký tự"
                }, JsonRequestBehavior.AllowGet);
            }

            var houseAreaMatch = Regex.Match(model.HouseArea, @"^[0-9]+\.?([0-9]+)?$", RegexOptions.IgnoreCase);

            if (!houseAreaMatch.Success)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích Nhà hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.ApartmentTypeId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "ApartmentTypeId",
                    Message = "Vui lòng chọn Loại Nhà"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.Year <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "Year",
                    Message = "Vui lòng chọn Năm xây dựng"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.ApartmentId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "ApartmentId",
                    Message = "Vui lòng chọn Cấp công trình"
                }, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrWhiteSpace(model.SubmitAction))
            {
                model.HouseArea = model.HouseArea.Replace(",", ".");

                Contracts contracts = await Contracts.Static_ContractDonationApartment(double.Parse(model.HouseArea, System.Globalization.CultureInfo.InvariantCulture), model.ApartmentTypeId, model.Year, model.ApartmentId);

                if (contracts != null && !string.IsNullOrWhiteSpace(contracts.ActionStatus) && contracts.ActionStatus.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('.row-ketqua').find('.gia-tri-nha-theo-khung-gia-nha-nuoc').first().html('{0:0,0}');$('.row-ketqua').find('.phi-cong-chung').first().html('{1:0,0}');$('.row-ketqua').removeClass('hidden');if($(window).scrollTop() > 150) $(\"html, body\").animate({{ scrollTop: $('.section-shadow').offset().top }});", contracts.AssetValue, contracts.NotarizationFee)))
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Completed = false,
                        Message = contracts.ActionMessage
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    Completed = true,
                    Data = "NoAction",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region LandUseRightTransferContract

        /// <summary>
        /// Hợp đồng chuyển nhượng quyền sử dụng Đất
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> LandUseRightTransferContract()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Districts>> districtsTask = Districts.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<LandTypes>> landTypesTask = LandTypes.Static_GetList();

                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask, districtsTask, landTypesTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result,
                    DistrictsList = districtsTask.Result,
                    LandTypesList = landTypesTask.Result
                };
            }

            LandUseRightTransferContractVM model = new LandUseRightTransferContractVM
            {
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList,
                DistrictsList = cacheVM.DistrictsList.IsAny() ? cacheVM.DistrictsList : new List<Districts>(),
                WardsList = new List<Wards>(),
                StreetsList = new List<Streets>(),
                LocationTypesList = new List<LocationTypes>(),
                LandTypesList = cacheVM.LandTypesList.IsAny() ? cacheVM.LandTypesList : new List<LandTypes>()
            };

            if (cacheVM.SeosList.IsAny())
            {
                model.Seos = cacheVM.SeosList.FirstOrDefault(x => Request.Url.AbsolutePath.GetUrl().Equals(x.Path.GetUrl()));

                if (model.Seos == null || model.Seos.SeoId <= 0 || string.IsNullOrWhiteSpace(model.Seos.Path))
                {
                    return Redirect(Url.Action("NotFound", "Home"));
                }

                if (!Request.Url.AbsolutePath.Contains(model.Seos.Path))
                {
                    return RedirectPermanent(model.Seos.Path.GetUrl());
                }

                model.WebsiteTitle = model.Seos.MetaTitle;
                model.MetaTitle = model.Seos.MetaTitle;
                model.MetaDescription = model.Seos.MetaDescription;
                model.MetaKeywords = model.Seos.MetaKeyword;
                model.SeoHeader = model.Seos.H1Tag;

                if (!string.IsNullOrWhiteSpace(model.Seos.Name))
                    model.SeoName = model.Seos.Name;

                if (!string.IsNullOrWhiteSpace(model.Seos.Description))
                    model.SeoDescription = model.Seos.Description;

                if (!string.IsNullOrWhiteSpace(model.Seos.SeoFooter))
                    model.SeoFooter = model.Seos.SeoFooter;

                if (!string.IsNullOrWhiteSpace(model.Seos.ImagePath))
                    model.WebsiteImage = model.Seos.ImagePath.GetImageUrl();
            }

            return View(model);
        }

        /// <summary>
        /// Hợp đồng chuyển nhượng quyền sử dụng Đất
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> LandUseRightTransferContract(LandUseRightTransferContractVM model)
        {
            if (model.DistrictId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "DistrictId",
                    Message = "Vui lòng chọn Quận/Huyện"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.WardId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "WardId",
                    Message = "Vui lòng chọn Phường/Xã"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.StreetId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "StreetId",
                    Message = "Vui lòng chọn Đường/Phố"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandTypeId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandTypeId",
                    Message = "Vui lòng chọn Loại đất"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LocationId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LocationId",
                    Message = "Vui lòng chọn Vị trí"
                }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrWhiteSpace(model.HouseArea))
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích Nhà"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.HouseArea.Trim().Length > 9)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích Nhà tối đa 9 ký tự"
                }, JsonRequestBehavior.AllowGet);
            }

            var houseAreaMatch = Regex.Match(model.HouseArea, @"^[0-9]+\.?([0-9]+)?$", RegexOptions.IgnoreCase);

            if (!houseAreaMatch.Success)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích Nhà hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrWhiteSpace(model.SubmitAction))
            {
                model.HouseArea = model.HouseArea.Replace(",", ".");

                Contracts contracts = await Contracts.Static_LandUseRightTransferContract(double.Parse(model.HouseArea, System.Globalization.CultureInfo.InvariantCulture), model.LocationId);

                if (contracts != null && !string.IsNullOrWhiteSpace(contracts.ActionStatus) && contracts.ActionStatus.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('.row-ketqua').find('.gia-tri-nha-theo-khung-gia-nha-nuoc').first().html('{0:0,0}');$('.row-ketqua').find('.phi-cong-chung').first().html('{1:0,0}');$('.row-ketqua').removeClass('hidden');if($(window).scrollTop() > 150) $(\"html, body\").animate({{ scrollTop: $('.section-shadow').offset().top }});", contracts.AssetValue, contracts.NotarizationFee)))
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Completed = false,
                        Message = contracts.ActionMessage
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    Completed = true,
                    Data = "NoAction",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region ContractSaleHouseAndTransferLandUseRight

        /// <summary>
        /// Hợp đồng mua bán Nhà và chuyển nhượng quyền sử dụng Đất
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ContractSaleHouseAndTransferLandUseRight()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Districts>> districtsTask = Districts.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<LandTypes>> landTypesTask = LandTypes.Static_GetList();
                Task<List<Lands>> landsTask = Lands.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<YearBuilts>> yearBuiltsTask = YearBuilts.Static_GetList(ConstantHelper.StatusIdActivated);

                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask, districtsTask, landTypesTask, landsTask, yearBuiltsTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result,
                    DistrictsList = districtsTask.Result,
                    LandTypesList = landTypesTask.Result,
                    LandsList = landsTask.Result,
                    YearBuiltsList = yearBuiltsTask.Result
                };
            }

            ContractSaleHouseAndTransferLandUseRightVM model = new ContractSaleHouseAndTransferLandUseRightVM
            {
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList,
                DistrictsList = cacheVM.DistrictsList.IsAny() ? cacheVM.DistrictsList : new List<Districts>(),
                WardsList = new List<Wards>(),
                StreetsList = new List<Streets>(),
                LocationTypesList = new List<LocationTypes>(),
                LandTypesList = cacheVM.LandTypesList.IsAny() ? cacheVM.LandTypesList : new List<LandTypes>(),
                LandsList = cacheVM.LandsList.IsAny() ? cacheVM.LandsList : new List<Lands>(),
                YearBuiltsList = cacheVM.YearBuiltsList.IsAny() ? cacheVM.YearBuiltsList : new List<YearBuilts>()
            };

            if (cacheVM.SeosList.IsAny())
            {
                model.Seos = cacheVM.SeosList.FirstOrDefault(x => Request.Url.AbsolutePath.GetUrl().Equals(x.Path.GetUrl()));

                if (model.Seos == null || model.Seos.SeoId <= 0 || string.IsNullOrWhiteSpace(model.Seos.Path))
                {
                    return Redirect(Url.Action("NotFound", "Home"));
                }

                if (!Request.Url.AbsolutePath.Contains(model.Seos.Path))
                {
                    return RedirectPermanent(model.Seos.Path.GetUrl());
                }

                model.WebsiteTitle = model.Seos.MetaTitle;
                model.MetaTitle = model.Seos.MetaTitle;
                model.MetaDescription = model.Seos.MetaDescription;
                model.MetaKeywords = model.Seos.MetaKeyword;
                model.SeoHeader = model.Seos.H1Tag;

                if (!string.IsNullOrWhiteSpace(model.Seos.Name))
                    model.SeoName = model.Seos.Name;

                if (!string.IsNullOrWhiteSpace(model.Seos.Description))
                    model.SeoDescription = model.Seos.Description;

                if (!string.IsNullOrWhiteSpace(model.Seos.SeoFooter))
                    model.SeoFooter = model.Seos.SeoFooter;

                if (!string.IsNullOrWhiteSpace(model.Seos.ImagePath))
                    model.WebsiteImage = model.Seos.ImagePath.GetImageUrl();
            }

            return View(model);
        }

        /// <summary>
        /// Hợp đồng mua bán Nhà và chuyển nhượng quyền sử dụng Đất
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ContractSaleHouseAndTransferLandUseRight(ContractSaleHouseAndTransferLandUseRightVM model)
        {
            if(model.DistrictId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "DistrictId",
                    Message = "Vui lòng chọn Quận/Huyện"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.WardId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "WardId",
                    Message = "Vui lòng chọn Phường/Xã"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.StreetId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "StreetId",
                    Message = "Vui lòng chọn Đường/Phố"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandTypeId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandTypeId",
                    Message = "Vui lòng chọn Loại đất"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LocationId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LocationId",
                    Message = "Vui lòng chọn Vị trí"
                }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrWhiteSpace(model.LandArea))
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Nhà"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandArea.Trim().Length > 9)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Nhà tối đa 9 ký tự"
                }, JsonRequestBehavior.AllowGet);
            }

            var landAreaMatch = Regex.Match(model.LandArea, @"^[0-9]+\.?([0-9]+)?$", RegexOptions.IgnoreCase);

            if (!landAreaMatch.Success)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Nhà hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrWhiteSpace(model.HouseArea))
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích sàn xây dựng"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.HouseArea.Trim().Length > 9)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích sàn xây dựng tối đa 9 ký tự"
                }, JsonRequestBehavior.AllowGet);
            }

            var houseAreaMatch = Regex.Match(model.HouseArea, @"^[0-9]+\.?([0-9]+)?$", RegexOptions.IgnoreCase);

            if (!houseAreaMatch.Success)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích sàn xây dựng hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.Year <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "Year",
                    Message = "Vui lòng chọn Năm xây dựng"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandId",
                    Message = "Vui lòng chọn Cấp công trình"
                }, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrWhiteSpace(model.SubmitAction))
            {
                model.LandArea = model.LandArea.Replace(",", ".");
                model.HouseArea = model.HouseArea.Replace(",", ".");

                Contracts contracts = await Contracts.Static_ContractSaleHouseAndTransferLandUseRight(double.Parse(model.HouseArea, System.Globalization.CultureInfo.InvariantCulture), double.Parse(model.LandArea, System.Globalization.CultureInfo.InvariantCulture), model.LandId, model.Year, model.LocationId);

                if (contracts != null && !string.IsNullOrWhiteSpace(contracts.ActionStatus) && contracts.ActionStatus.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('.row-ketqua').find('.gia-tri-nha-theo-khung-gia-nha-nuoc').first().html('{0:0,0}');$('.row-ketqua').find('.phi-cong-chung').first().html('{1:0,0}');$('.row-ketqua').removeClass('hidden');if($(window).scrollTop() > 150) $(\"html, body\").animate({{ scrollTop: $('.section-shadow').offset().top }});", contracts.AssetValue, contracts.NotarizationFee)))
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Completed = false,
                        Message = contracts.ActionMessage
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    Completed = true,
                    Data = "NoAction",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region ContractDonationHouseLand

        /// <summary>
        /// Hợp đồng tặng cho Nhà - Đất
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ContractDonationHouseLand()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Districts>> districtsTask = Districts.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<LandTypes>> landTypesTask = LandTypes.Static_GetList();
                Task<List<Lands>> landsTask = Lands.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<YearBuilts>> yearBuiltsTask = YearBuilts.Static_GetList(ConstantHelper.StatusIdActivated);

                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask, districtsTask, landTypesTask, landsTask, yearBuiltsTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result,
                    DistrictsList = districtsTask.Result,
                    LandsList = landsTask.Result,
                    LandTypesList = landTypesTask.Result,
                    YearBuiltsList = yearBuiltsTask.Result
                };
            }

            ContractDonationHouseLandVM model = new ContractDonationHouseLandVM
            {
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList,
                DistrictsList = cacheVM.DistrictsList.IsAny() ? cacheVM.DistrictsList : new List<Districts>(),
                WardsList = new List<Wards>(),
                StreetsList = new List<Streets>(),
                LocationTypesList = new List<LocationTypes>(),
                LandTypesList = cacheVM.LandTypesList.IsAny() ? cacheVM.LandTypesList : new List<LandTypes>(),
                LandsList = cacheVM.LandsList.IsAny() ? cacheVM.LandsList : new List<Lands>(),
                YearBuiltsList = cacheVM.YearBuiltsList.IsAny() ? cacheVM.YearBuiltsList : new List<YearBuilts>()
            };

            if (cacheVM.SeosList.IsAny())
            {
                model.Seos = cacheVM.SeosList.FirstOrDefault(x => Request.Url.AbsolutePath.GetUrl().Equals(x.Path.GetUrl()));

                if (model.Seos == null || model.Seos.SeoId <= 0 || string.IsNullOrWhiteSpace(model.Seos.Path))
                {
                    return Redirect(Url.Action("NotFound", "Home"));
                }

                if (!Request.Url.AbsolutePath.Contains(model.Seos.Path))
                {
                    return RedirectPermanent(model.Seos.Path.GetUrl());
                }

                model.WebsiteTitle = model.Seos.MetaTitle;
                model.MetaTitle = model.Seos.MetaTitle;
                model.MetaDescription = model.Seos.MetaDescription;
                model.MetaKeywords = model.Seos.MetaKeyword;
                model.SeoHeader = model.Seos.H1Tag;

                if (!string.IsNullOrWhiteSpace(model.Seos.Name))
                    model.SeoName = model.Seos.Name;

                if (!string.IsNullOrWhiteSpace(model.Seos.Description))
                    model.SeoDescription = model.Seos.Description;

                if (!string.IsNullOrWhiteSpace(model.Seos.SeoFooter))
                    model.SeoFooter = model.Seos.SeoFooter;

                if (!string.IsNullOrWhiteSpace(model.Seos.ImagePath))
                    model.WebsiteImage = model.Seos.ImagePath.GetImageUrl();
            }

            return View(model);
        }

        /// <summary>
        /// Hợp đồng tặng cho Nhà - Đất
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ContractDonationHouseLand(ContractDonationHouseLandVM model)
        {
            if (model.DistrictId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "DistrictId",
                    Message = "Vui lòng chọn Quận/Huyện"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.WardId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "WardId",
                    Message = "Vui lòng chọn Phường/Xã"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.StreetId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "StreetId",
                    Message = "Vui lòng chọn Đường/Phố"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandTypeId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandTypeId",
                    Message = "Vui lòng chọn Loại đất"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LocationId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LocationId",
                    Message = "Vui lòng chọn Vị trí"
                }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrWhiteSpace(model.LandArea))
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Đất"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandArea.Trim().Length > 9)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Đất tối đa 9 ký tự"
                }, JsonRequestBehavior.AllowGet);
            }

            var landAreaMatch = Regex.Match(model.LandArea, @"^[0-9]+\.?([0-9]+)?$", RegexOptions.IgnoreCase);

            if (!landAreaMatch.Success)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Đất hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrWhiteSpace(model.HouseArea))
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích sàn xây dựng"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.HouseArea.Trim().Length > 9)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích sàn xây dựng tối đa 9 ký tự"
                }, JsonRequestBehavior.AllowGet);
            }

            var houseAreaMatch = Regex.Match(model.HouseArea, @"^[0-9]+\.?([0-9]+)?$", RegexOptions.IgnoreCase);

            if (!houseAreaMatch.Success)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích sàn xây dựng hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.Year <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "Year",
                    Message = "Vui lòng chọn Năm xây dựng"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandId",
                    Message = "Vui lòng chọn Cấp công trình"
                }, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrWhiteSpace(model.SubmitAction))
            {
                model.LandArea = model.LandArea.Replace(",", ".");
                model.HouseArea = model.HouseArea.Replace(",", ".");

                Contracts contracts = await Contracts.Static_ContractSaleHouseAndTransferLandUseRight(double.Parse(model.HouseArea, System.Globalization.CultureInfo.InvariantCulture), double.Parse(model.LandArea, System.Globalization.CultureInfo.InvariantCulture), model.LandId, model.Year, model.LocationId);

                if (contracts != null && !string.IsNullOrWhiteSpace(contracts.ActionStatus) && contracts.ActionStatus.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('.row-ketqua').find('.gia-tri-nha-theo-khung-gia-nha-nuoc').first().html('{0:0,0}');$('.row-ketqua').find('.phi-cong-chung').first().html('{1:0,0}');$('.row-ketqua').removeClass('hidden');if($(window).scrollTop() > 150) $(\"html, body\").animate({{ scrollTop: $('.section-shadow').offset().top }});", contracts.AssetValue, contracts.NotarizationFee)))
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Completed = false,
                        Message = contracts.ActionMessage
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    Completed = true,
                    Data = "NoAction",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region ContractDonationLandUseRight

        /// <summary>
        /// Hợp đồng tặng cho quyền sử dụng Đất
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ContractDonationLandUseRight()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Districts>> districtsTask = Districts.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<LandTypes>> landTypesTask = LandTypes.Static_GetList();

                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask, districtsTask, landTypesTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result,
                    DistrictsList = districtsTask.Result,
                    LandTypesList = landTypesTask.Result
                };
            }

            ContractDonationLandUseRightVM model = new ContractDonationLandUseRightVM
            {
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList,
                DistrictsList = cacheVM.DistrictsList.IsAny() ? cacheVM.DistrictsList : new List<Districts>(),
                WardsList = new List<Wards>(),
                StreetsList = new List<Streets>(),
                LocationTypesList = new List<LocationTypes>(),
                LandTypesList = cacheVM.LandTypesList.IsAny() ? cacheVM.LandTypesList : new List<LandTypes>()
            };

            if (cacheVM.SeosList.IsAny())
            {
                model.Seos = cacheVM.SeosList.FirstOrDefault(x => Request.Url.AbsolutePath.GetUrl().Equals(x.Path.GetUrl()));

                if (model.Seos == null || model.Seos.SeoId <= 0 || string.IsNullOrWhiteSpace(model.Seos.Path))
                {
                    return Redirect(Url.Action("NotFound", "Home"));
                }

                if (!Request.Url.AbsolutePath.Contains(model.Seos.Path))
                {
                    return RedirectPermanent(model.Seos.Path.GetUrl());
                }

                model.WebsiteTitle = model.Seos.MetaTitle;
                model.MetaTitle = model.Seos.MetaTitle;
                model.MetaDescription = model.Seos.MetaDescription;
                model.MetaKeywords = model.Seos.MetaKeyword;
                model.SeoHeader = model.Seos.H1Tag;

                if (!string.IsNullOrWhiteSpace(model.Seos.Name))
                    model.SeoName = model.Seos.Name;

                if (!string.IsNullOrWhiteSpace(model.Seos.Description))
                    model.SeoDescription = model.Seos.Description;

                if (!string.IsNullOrWhiteSpace(model.Seos.SeoFooter))
                    model.SeoFooter = model.Seos.SeoFooter;

                if (!string.IsNullOrWhiteSpace(model.Seos.ImagePath))
                    model.WebsiteImage = model.Seos.ImagePath.GetImageUrl();
            }

            return View(model);
        }

        /// <summary>
        /// Hợp đồng tặng cho quyền sử dụng Đất
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ContractDonationLandUseRight(ContractDonationLandUseRightVM model)
        {
            if (model.DistrictId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "DistrictId",
                    Message = "Vui lòng chọn Quận/Huyện"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.WardId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "WardId",
                    Message = "Vui lòng chọn Phường/Xã"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.StreetId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "StreetId",
                    Message = "Vui lòng chọn Đường/Phố"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandTypeId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandTypeId",
                    Message = "Vui lòng chọn Loại đất"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LocationId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LocationId",
                    Message = "Vui lòng chọn Vị trí"
                }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrWhiteSpace(model.LandArea))
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Đất"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandArea.Trim().Length > 9)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Vui lòng nhập Diện tích Đất tối đa 9 ký tự"
                }, JsonRequestBehavior.AllowGet);
            }

            var landAreaMatch = Regex.Match(model.LandArea, @"^[0-9]+\.?([0-9]+)?$", RegexOptions.IgnoreCase);

            if (!landAreaMatch.Success)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Vui lòng nhập Diện tích Đất hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrWhiteSpace(model.SubmitAction))
            {
                model.LandArea = model.LandArea.Replace(",", ".");

                Contracts contracts = await Contracts.Static_ContractDonationLandUseRight(double.Parse(model.LandArea, System.Globalization.CultureInfo.InvariantCulture), model.LocationId);

                if (contracts != null && !string.IsNullOrWhiteSpace(contracts.ActionStatus) && contracts.ActionStatus.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('.row-ketqua').find('.gia-tri-nha-theo-khung-gia-nha-nuoc').first().html('{0:0,0}');$('.row-ketqua').find('.phi-cong-chung').first().html('{1:0,0}');$('.row-ketqua').removeClass('hidden');if($(window).scrollTop() > 150) $(\"html, body\").animate({{ scrollTop: $('.section-shadow').offset().top }});", contracts.AssetValue, contracts.NotarizationFee)))
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Completed = false,
                        Message = contracts.ActionMessage
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    Completed = true,
                    Data = "NoAction",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region HouseLandSeparationContract

        /// <summary>
        /// Hợp đồng chia tách Nhà - Đất
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> HouseLandSeparationContract()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Districts>> districtsTask = Districts.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<LandTypes>> landTypesTask = LandTypes.Static_GetList();
                Task<List<Lands>> landsTask = Lands.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<YearBuilts>> yearBuiltsTask = YearBuilts.Static_GetList(ConstantHelper.StatusIdActivated);

                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask, districtsTask, landTypesTask, landsTask, yearBuiltsTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result,
                    DistrictsList = districtsTask.Result,
                    LandTypesList = landTypesTask.Result,
                    LandsList = landsTask.Result,
                    YearBuiltsList = yearBuiltsTask.Result
                };
            }

            HouseLandSeparationContractVM model = new HouseLandSeparationContractVM
            {
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList,
                DistrictsList = cacheVM.DistrictsList.IsAny() ? cacheVM.DistrictsList : new List<Districts>(),
                WardsList = new List<Wards>(),
                StreetsList = new List<Streets>(),
                LocationTypesList = new List<LocationTypes>(),
                LandTypesList = cacheVM.LandTypesList.IsAny() ? cacheVM.LandTypesList : new List<LandTypes>(),
                LandsList = cacheVM.LandsList.IsAny() ? cacheVM.LandsList : new List<Lands>(),
                YearBuiltsList = cacheVM.YearBuiltsList.IsAny() ? cacheVM.YearBuiltsList : new List<YearBuilts>()
            };

            if (cacheVM.SeosList.IsAny())
            {
                model.Seos = cacheVM.SeosList.FirstOrDefault(x => Request.Url.AbsolutePath.GetUrl().Equals(x.Path.GetUrl()));

                if (model.Seos == null || model.Seos.SeoId <= 0 || string.IsNullOrWhiteSpace(model.Seos.Path))
                {
                    return Redirect(Url.Action("NotFound", "Home"));
                }

                if (!Request.Url.AbsolutePath.Contains(model.Seos.Path))
                {
                    return RedirectPermanent(model.Seos.Path.GetUrl());
                }

                model.WebsiteTitle = model.Seos.MetaTitle;
                model.MetaTitle = model.Seos.MetaTitle;
                model.MetaDescription = model.Seos.MetaDescription;
                model.MetaKeywords = model.Seos.MetaKeyword;
                model.SeoHeader = model.Seos.H1Tag;

                if (!string.IsNullOrWhiteSpace(model.Seos.Name))
                    model.SeoName = model.Seos.Name;

                if (!string.IsNullOrWhiteSpace(model.Seos.Description))
                    model.SeoDescription = model.Seos.Description;

                if (!string.IsNullOrWhiteSpace(model.Seos.SeoFooter))
                    model.SeoFooter = model.Seos.SeoFooter;

                if (!string.IsNullOrWhiteSpace(model.Seos.ImagePath))
                    model.WebsiteImage = model.Seos.ImagePath.GetImageUrl();
            }

            return View(model);
        }

        /// <summary>
        /// Hợp đồng chia tách Nhà - Đất
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> HouseLandSeparationContract(HouseLandSeparationContractVM model)
        {
            if (model.DistrictId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "DistrictId",
                    Message = "Vui lòng chọn Quận/Huyện"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.WardId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "WardId",
                    Message = "Vui lòng chọn Phường/Xã"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.StreetId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "StreetId",
                    Message = "Vui lòng chọn Đường/Phố"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandTypeId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandTypeId",
                    Message = "Vui lòng chọn Loại đất"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LocationId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LocationId",
                    Message = "Vui lòng chọn Vị trí"
                }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrWhiteSpace(model.LandArea))
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Đất"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandArea.Trim().Length > 9)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Đất tối đa 9 ký tự"
                }, JsonRequestBehavior.AllowGet);
            }

            var landAreaMatch = Regex.Match(model.LandArea, @"^[0-9]+\.?([0-9]+)?$", RegexOptions.IgnoreCase);

            if (!landAreaMatch.Success)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Đất hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrWhiteSpace(model.HouseArea))
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích sàn xây dựng"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.HouseArea.Trim().Length > 9)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích sàn xây dựng tối đa 9 ký tự"
                }, JsonRequestBehavior.AllowGet);
            }

            var houseAreaMatch = Regex.Match(model.HouseArea, @"^[0-9]+\.?([0-9]+)?$", RegexOptions.IgnoreCase);

            if (!houseAreaMatch.Success)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích sàn xây dựng hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.Year <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "Year",
                    Message = "Vui lòng chọn Năm xây dựng"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandId",
                    Message = "Vui lòng chọn Cấp công trình"
                }, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrWhiteSpace(model.SubmitAction))
            {
                model.LandArea = model.LandArea.Replace(",", ".");
                model.HouseArea = model.HouseArea.Replace(",", ".");

                Contracts contracts = await Contracts.Static_HouseLandSeparationContract(double.Parse(model.HouseArea, System.Globalization.CultureInfo.InvariantCulture), double.Parse(model.LandArea, System.Globalization.CultureInfo.InvariantCulture), model.LandId, model.Year, model.LocationId);

                if (contracts != null && !string.IsNullOrWhiteSpace(contracts.ActionStatus) && contracts.ActionStatus.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('.row-ketqua').find('.gia-tri-nha-theo-khung-gia-nha-nuoc').first().html('{0:0,0}');$('.row-ketqua').find('.phi-cong-chung').first().html('{1:0,0}');$('.row-ketqua').removeClass('hidden');if($(window).scrollTop() > 150) $(\"html, body\").animate({{ scrollTop: $('.section-shadow').offset().top }});", contracts.AssetValue, contracts.NotarizationFee)))
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Completed = false,
                        Message = contracts.ActionMessage
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    Completed = true,
                    Data = "NoAction",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region ContractDivisionLandUseRight

        /// <summary>
        /// Hợp đồng chia tách quyền sử dụng Đất
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ContractDivisionLandUseRight()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Districts>> districtsTask = Districts.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<LandTypes>> landTypesTask = LandTypes.Static_GetList();

                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask, districtsTask, landTypesTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result,
                    DistrictsList = districtsTask.Result,
                    LandTypesList = landTypesTask.Result
                };
            }

            ContractDivisionLandUseRightVM model = new ContractDivisionLandUseRightVM
            {
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList,
                DistrictsList = cacheVM.DistrictsList.IsAny() ? cacheVM.DistrictsList : new List<Districts>(),
                WardsList = new List<Wards>(),
                StreetsList = new List<Streets>(),
                LocationTypesList = new List<LocationTypes>(),
                LandTypesList = cacheVM.LandTypesList.IsAny() ? cacheVM.LandTypesList : new List<LandTypes>()
            };

            if (cacheVM.SeosList.IsAny())
            {
                model.Seos = cacheVM.SeosList.FirstOrDefault(x => Request.Url.AbsolutePath.GetUrl().Equals(x.Path.GetUrl()));

                if (model.Seos == null || model.Seos.SeoId <= 0 || string.IsNullOrWhiteSpace(model.Seos.Path))
                {
                    return Redirect(Url.Action("NotFound", "Home"));
                }

                if (!Request.Url.AbsolutePath.Contains(model.Seos.Path))
                {
                    return RedirectPermanent(model.Seos.Path.GetUrl());
                }

                model.WebsiteTitle = model.Seos.MetaTitle;
                model.MetaTitle = model.Seos.MetaTitle;
                model.MetaDescription = model.Seos.MetaDescription;
                model.MetaKeywords = model.Seos.MetaKeyword;
                model.SeoHeader = model.Seos.H1Tag;

                if (!string.IsNullOrWhiteSpace(model.Seos.Name))
                    model.SeoName = model.Seos.Name;

                if (!string.IsNullOrWhiteSpace(model.Seos.Description))
                    model.SeoDescription = model.Seos.Description;

                if (!string.IsNullOrWhiteSpace(model.Seos.SeoFooter))
                    model.SeoFooter = model.Seos.SeoFooter;

                if (!string.IsNullOrWhiteSpace(model.Seos.ImagePath))
                    model.WebsiteImage = model.Seos.ImagePath.GetImageUrl();
            }

            return View(model);
        }

        /// <summary>
        /// Hợp đồng chia tách quyền sử dụng Đất
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ContractDivisionLandUseRight(ContractDivisionLandUseRightVM model)
        {
            if (model.DistrictId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "DistrictId",
                    Message = "Vui lòng chọn Quận/Huyện"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.WardId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "WardId",
                    Message = "Vui lòng chọn Phường/Xã"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.StreetId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "StreetId",
                    Message = "Vui lòng chọn Đường/Phố"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandTypeId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandTypeId",
                    Message = "Vui lòng chọn Loại đất"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LocationId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LocationId",
                    Message = "Vui lòng chọn Vị trí"
                }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrWhiteSpace(model.LandArea))
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Đất"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandArea.Trim().Length > 9)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Đất tối đa 9 ký tự"
                }, JsonRequestBehavior.AllowGet);
            }

            var landAreaMatch = Regex.Match(model.LandArea, @"^[0-9]+\.?([0-9]+)?$", RegexOptions.IgnoreCase);

            if (!landAreaMatch.Success)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Đất hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }


            if (!string.IsNullOrWhiteSpace(model.SubmitAction))
            {
                model.LandArea = model.LandArea.Replace(",", ".");

                Contracts contracts = await Contracts.Static_LandUseRightTransferContract(double.Parse(model.LandArea, System.Globalization.CultureInfo.InvariantCulture), model.LocationId);

                if (contracts != null && !string.IsNullOrWhiteSpace(contracts.ActionStatus) && contracts.ActionStatus.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('.row-ketqua').find('.gia-tri-nha-theo-khung-gia-nha-nuoc').first().html('{0:0,0}');$('.row-ketqua').find('.phi-cong-chung').first().html('{1:0,0}');$('.row-ketqua').removeClass('hidden');if($(window).scrollTop() > 150) $(\"html, body\").animate({{ scrollTop: $('.section-shadow').offset().top }});", contracts.AssetValue, contracts.NotarizationFee)))
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Completed = false,
                        Message = contracts.ActionMessage
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    Completed = true,
                    Data = "NoAction",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region ContractCapitalContributionByHouseLand

        /// <summary>
        /// Hợp đồng góp vốn bằng Nhà - Đất
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ContractCapitalContributionByHouseLand()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Districts>> districtsTask = Districts.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<LandTypes>> landTypesTask = LandTypes.Static_GetList();
                Task<List<Lands>> landsTask = Lands.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<YearBuilts>> yearBuiltsTask = YearBuilts.Static_GetList(ConstantHelper.StatusIdActivated);

                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask, districtsTask, landTypesTask, landsTask, yearBuiltsTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result,
                    DistrictsList = districtsTask.Result,
                    LandsList = landsTask.Result,
                    LandTypesList = landTypesTask.Result,
                    YearBuiltsList = yearBuiltsTask.Result
                };
            }

            ContractCapitalContributionByHouseLandVM model = new ContractCapitalContributionByHouseLandVM
            {
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList,
                DistrictsList = cacheVM.DistrictsList.IsAny() ? cacheVM.DistrictsList : new List<Districts>(),
                WardsList = new List<Wards>(),
                StreetsList = new List<Streets>(),
                LocationTypesList = new List<LocationTypes>(),
                LandTypesList = cacheVM.LandTypesList.IsAny() ? cacheVM.LandTypesList : new List<LandTypes>(),
                LandsList = cacheVM.LandsList.IsAny() ? cacheVM.LandsList : new List<Lands>(),
                YearBuiltsList = cacheVM.YearBuiltsList.IsAny() ? cacheVM.YearBuiltsList : new List<YearBuilts>()
            };

            if (cacheVM.SeosList.IsAny())
            {
                model.Seos = cacheVM.SeosList.FirstOrDefault(x => Request.Url.AbsolutePath.GetUrl().Equals(x.Path.GetUrl()));

                if (model.Seos == null || model.Seos.SeoId <= 0 || string.IsNullOrWhiteSpace(model.Seos.Path))
                {
                    return Redirect(Url.Action("NotFound", "Home"));
                }

                if (!Request.Url.AbsolutePath.Contains(model.Seos.Path))
                {
                    return RedirectPermanent(model.Seos.Path.GetUrl());
                }

                model.WebsiteTitle = model.Seos.MetaTitle;
                model.MetaTitle = model.Seos.MetaTitle;
                model.MetaDescription = model.Seos.MetaDescription;
                model.MetaKeywords = model.Seos.MetaKeyword;
                model.SeoHeader = model.Seos.H1Tag;

                if (!string.IsNullOrWhiteSpace(model.Seos.Name))
                    model.SeoName = model.Seos.Name;

                if (!string.IsNullOrWhiteSpace(model.Seos.Description))
                    model.SeoDescription = model.Seos.Description;

                if (!string.IsNullOrWhiteSpace(model.Seos.SeoFooter))
                    model.SeoFooter = model.Seos.SeoFooter;

                if (!string.IsNullOrWhiteSpace(model.Seos.ImagePath))
                    model.WebsiteImage = model.Seos.ImagePath.GetImageUrl();
            }

            return View(model);
        }

        /// <summary>
        /// Hợp đồng góp vốn bằng Nhà - Đất
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ContractCapitalContributionByHouseLand(ContractCapitalContributionByHouseLandVM model)
        {
            if (model.DistrictId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "DistrictId",
                    Message = "Vui lòng chọn Quận/Huyện"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.WardId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "WardId",
                    Message = "Vui lòng chọn Phường/Xã"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.StreetId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "StreetId",
                    Message = "Vui lòng chọn Đường/Phố"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandTypeId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandTypeId",
                    Message = "Vui lòng chọn Loại đất"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LocationId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LocationId",
                    Message = "Vui lòng chọn Vị trí"
                }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrWhiteSpace(model.LandArea))
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Đất"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandArea.Trim().Length > 9)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Đất tối đa 9 ký tự"
                }, JsonRequestBehavior.AllowGet);
            }

            var landAreaMatch = Regex.Match(model.LandArea, @"^[0-9]+\.?([0-9]+)?$", RegexOptions.IgnoreCase);

            if (!landAreaMatch.Success)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Đất hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrWhiteSpace(model.HouseArea))
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích sàn xây dựng"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.HouseArea.Trim().Length > 9)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích sàn xây dựng tối đa 9 ký tự"
                }, JsonRequestBehavior.AllowGet);
            }

            var houseAreaMatch = Regex.Match(model.HouseArea, @"^[0-9]+\.?([0-9]+)?$", RegexOptions.IgnoreCase);

            if (!houseAreaMatch.Success)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích sàn xây dựng hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.Year <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "Year",
                    Message = "Vui lòng chọn Năm xây dựng"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandId",
                    Message = "Vui lòng chọn Cấp công trình"
                }, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrWhiteSpace(model.SubmitAction))
            {
                model.LandArea = model.LandArea.Replace(",", ".");
                model.HouseArea = model.HouseArea.Replace(",", ".");

                Contracts contracts = await Contracts.Static_ContractCapitalContributionByHouseLand(double.Parse(model.HouseArea, System.Globalization.CultureInfo.InvariantCulture), double.Parse(model.LandArea, System.Globalization.CultureInfo.InvariantCulture), model.LandId, model.Year, model.LocationId);

                if (contracts != null && !string.IsNullOrWhiteSpace(contracts.ActionStatus) && contracts.ActionStatus.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('.row-ketqua').find('.gia-tri-nha-theo-khung-gia-nha-nuoc').first().html('{0:0,0}');$('.row-ketqua').find('.phi-cong-chung').first().html('{1:0,0}');$('.row-ketqua').removeClass('hidden');if($(window).scrollTop() > 150) $(\"html, body\").animate({{ scrollTop: $('.section-shadow').offset().top }});", contracts.AssetValue, contracts.NotarizationFee)))
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Completed = false,
                        Message = contracts.ActionMessage
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    Completed = true,
                    Data = "NoAction",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region ContractCapitalContributionByLandUseRight

        /// <summary>
        /// Hợp đồng góp vốn bằng quyền sử dụng Đất
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> ContractCapitalContributionByLandUseRight()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Districts>> districtsTask = Districts.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<LandTypes>> landTypesTask = LandTypes.Static_GetList();

                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask, districtsTask, landTypesTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result,
                    DistrictsList = districtsTask.Result,
                    LandTypesList = landTypesTask.Result
                };
            }

            ContractCapitalContributionByLandUseRightVM model = new ContractCapitalContributionByLandUseRightVM
            {
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList,
                DistrictsList = cacheVM.DistrictsList.IsAny() ? cacheVM.DistrictsList : new List<Districts>(),
                WardsList = new List<Wards>(),
                StreetsList = new List<Streets>(),
                LocationTypesList = new List<LocationTypes>(),
                LandTypesList = cacheVM.LandTypesList.IsAny() ? cacheVM.LandTypesList : new List<LandTypes>()
            };

            if (cacheVM.SeosList.IsAny())
            {
                model.Seos = cacheVM.SeosList.FirstOrDefault(x => Request.Url.AbsolutePath.GetUrl().Equals(x.Path.GetUrl()));

                if (model.Seos == null || model.Seos.SeoId <= 0 || string.IsNullOrWhiteSpace(model.Seos.Path))
                {
                    return Redirect(Url.Action("NotFound", "Home"));
                }

                if (!Request.Url.AbsolutePath.Contains(model.Seos.Path))
                {
                    return RedirectPermanent(model.Seos.Path.GetUrl());
                }

                model.WebsiteTitle = model.Seos.MetaTitle;
                model.MetaTitle = model.Seos.MetaTitle;
                model.MetaDescription = model.Seos.MetaDescription;
                model.MetaKeywords = model.Seos.MetaKeyword;
                model.SeoHeader = model.Seos.H1Tag;

                if (!string.IsNullOrWhiteSpace(model.Seos.Name))
                    model.SeoName = model.Seos.Name;

                if (!string.IsNullOrWhiteSpace(model.Seos.Description))
                    model.SeoDescription = model.Seos.Description;

                if (!string.IsNullOrWhiteSpace(model.Seos.SeoFooter))
                    model.SeoFooter = model.Seos.SeoFooter;

                if (!string.IsNullOrWhiteSpace(model.Seos.ImagePath))
                    model.WebsiteImage = model.Seos.ImagePath.GetImageUrl();
            }

            return View(model);
        }

        /// <summary>
        /// Hợp đồng góp vốn bằng quyền sử dụng Đất
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ContractCapitalContributionByLandUseRight(ContractCapitalContributionByLandUseRightVM model)
        {
            if (model.DistrictId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "DistrictId",
                    Message = "Vui lòng chọn Quận/Huyện"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.WardId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "WardId",
                    Message = "Vui lòng chọn Phường/Xã"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.StreetId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "StreetId",
                    Message = "Vui lòng chọn Đường/Phố"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandTypeId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandTypeId",
                    Message = "Vui lòng chọn Loại đất"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LocationId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LocationId",
                    Message = "Vui lòng chọn Vị trí"
                }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrWhiteSpace(model.LandArea))
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Đất"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandArea.Trim().Length > 9)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Đất tối đa 9 ký tự"
                }, JsonRequestBehavior.AllowGet);
            }

            var landAreaMatch = Regex.Match(model.LandArea, @"^[0-9]+\.?([0-9]+)?$", RegexOptions.IgnoreCase);

            if (!landAreaMatch.Success)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Đất hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrWhiteSpace(model.SubmitAction))
            {
                model.LandArea = model.LandArea.Replace(",", ".");

                Contracts contracts = await Contracts.Static_ContractCapitalContributionByLandUseRight(double.Parse(model.LandArea, System.Globalization.CultureInfo.InvariantCulture), model.LocationId);

                if (contracts != null && !string.IsNullOrWhiteSpace(contracts.ActionStatus) && contracts.ActionStatus.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('.row-ketqua').find('.gia-tri-nha-theo-khung-gia-nha-nuoc').first().html('{0:0,0}');$('.row-ketqua').find('.phi-cong-chung').first().html('{1:0,0}');$('.row-ketqua').removeClass('hidden');if($(window).scrollTop() > 150) $(\"html, body\").animate({{ scrollTop: $('.section-shadow').offset().top }});", contracts.AssetValue, contracts.NotarizationFee)))
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Completed = false,
                        Message = contracts.ActionMessage
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    Completed = true,
                    Data = "NoAction",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region DocumentDivisionInheritanceLandUseRight

        /// <summary>
        /// Văn bản chia di sản thừa kế là quyền sử dụng Đất
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> DocumentDivisionInheritanceLandUseRight()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Districts>> districtsTask = Districts.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<LandTypes>> landTypesTask = LandTypes.Static_GetList();

                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask, districtsTask, landTypesTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result,
                    DistrictsList = districtsTask.Result,
                    LandTypesList = landTypesTask.Result
                };
            }

            DocumentDivisionInheritanceLandUseRightVM model = new DocumentDivisionInheritanceLandUseRightVM
            {
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList,
                DistrictsList = cacheVM.DistrictsList.IsAny() ? cacheVM.DistrictsList : new List<Districts>(),
                WardsList = new List<Wards>(),
                StreetsList = new List<Streets>(),
                LocationTypesList = new List<LocationTypes>(),
                LandTypesList = cacheVM.LandTypesList.IsAny() ? cacheVM.LandTypesList : new List<LandTypes>()
            };

            if (cacheVM.SeosList.IsAny())
            {
                model.Seos = cacheVM.SeosList.FirstOrDefault(x => Request.Url.AbsolutePath.GetUrl().Equals(x.Path.GetUrl()));

                if (model.Seos == null || model.Seos.SeoId <= 0 || string.IsNullOrWhiteSpace(model.Seos.Path))
                {
                    return Redirect(Url.Action("NotFound", "Home"));
                }

                if (!Request.Url.AbsolutePath.Contains(model.Seos.Path))
                {
                    return RedirectPermanent(model.Seos.Path.GetUrl());
                }

                model.WebsiteTitle = model.Seos.MetaTitle;
                model.MetaTitle = model.Seos.MetaTitle;
                model.MetaDescription = model.Seos.MetaDescription;
                model.MetaKeywords = model.Seos.MetaKeyword;
                model.SeoHeader = model.Seos.H1Tag;

                if (!string.IsNullOrWhiteSpace(model.Seos.Name))
                    model.SeoName = model.Seos.Name;

                if (!string.IsNullOrWhiteSpace(model.Seos.Description))
                    model.SeoDescription = model.Seos.Description;

                if (!string.IsNullOrWhiteSpace(model.Seos.SeoFooter))
                    model.SeoFooter = model.Seos.SeoFooter;

                if (!string.IsNullOrWhiteSpace(model.Seos.ImagePath))
                    model.WebsiteImage = model.Seos.ImagePath.GetImageUrl();
            }

            return View(model);
        }

        /// <summary>
        /// Văn bản chia di sản thừa kế là quyền sử dụng Đất
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DocumentDivisionInheritanceLandUseRight(DocumentDivisionInheritanceLandUseRightVM model)
        {
            if (model.DistrictId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "DistrictId",
                    Message = "Vui lòng chọn Quận/Huyện"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.WardId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "WardId",
                    Message = "Vui lòng chọn Phường/Xã"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.StreetId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "StreetId",
                    Message = "Vui lòng chọn Đường/Phố"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandTypeId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandTypeId",
                    Message = "Vui lòng chọn Loại đất"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LocationId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LocationId",
                    Message = "Vui lòng chọn Vị trí"
                }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrWhiteSpace(model.LandArea))
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Đất"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandArea.Trim().Length > 9)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Đất tối đa 9 ký tự"
                }, JsonRequestBehavior.AllowGet);
            }

            var landAreaMatch = Regex.Match(model.LandArea, @"^[0-9]+\.?([0-9]+)?$", RegexOptions.IgnoreCase);

            if (!landAreaMatch.Success)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Đất hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrWhiteSpace(model.SubmitAction))
            {
                model.LandArea = model.LandArea.Replace(",", ".");

                Contracts contracts = await Contracts.Static_DocumentDivisionInheritanceLandUseRight(double.Parse(model.LandArea, System.Globalization.CultureInfo.InvariantCulture), model.LocationId);

                if (contracts != null && !string.IsNullOrWhiteSpace(contracts.ActionStatus) && contracts.ActionStatus.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('.row-ketqua').find('.gia-tri-nha-theo-khung-gia-nha-nuoc').first().html('{0:0,0}');$('.row-ketqua').find('.phi-cong-chung').first().html('{1:0,0}');$('.row-ketqua').removeClass('hidden');if($(window).scrollTop() > 150) $(\"html, body\").animate({{ scrollTop: $('.section-shadow').offset().top }});", contracts.AssetValue, contracts.NotarizationFee)))
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Completed = false,
                        Message = contracts.ActionMessage
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    Completed = true,
                    Data = "NoAction",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region DocumentDivisionInheritanceHouseLand

        /// <summary>
        /// Văn bản chia di sản thừa kế là Nhà - Đất
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> DocumentDivisionInheritanceHouseLand()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Districts>> districtsTask = Districts.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<LandTypes>> landTypesTask = LandTypes.Static_GetList();
                Task<List<Lands>> landsTask = Lands.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<YearBuilts>> yearBuiltsTask = YearBuilts.Static_GetList(ConstantHelper.StatusIdActivated);

                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask, districtsTask, landTypesTask, landsTask, yearBuiltsTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result,
                    DistrictsList = districtsTask.Result,
                    LandsList = landsTask.Result,
                    LandTypesList = landTypesTask.Result,
                    YearBuiltsList = yearBuiltsTask.Result
                };
            }

            DocumentDivisionInheritanceHouseLandVM model = new DocumentDivisionInheritanceHouseLandVM
            {
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList,
                DistrictsList = cacheVM.DistrictsList.IsAny() ? cacheVM.DistrictsList : new List<Districts>(),
                WardsList = new List<Wards>(),
                StreetsList = new List<Streets>(),
                LocationTypesList = new List<LocationTypes>(),
                LandTypesList = cacheVM.LandTypesList.IsAny() ? cacheVM.LandTypesList : new List<LandTypes>(),
                LandsList = cacheVM.LandsList.IsAny() ? cacheVM.LandsList : new List<Lands>(),
                YearBuiltsList = cacheVM.YearBuiltsList.IsAny() ? cacheVM.YearBuiltsList : new List<YearBuilts>()
            };

            if (cacheVM.SeosList.IsAny())
            {
                model.Seos = cacheVM.SeosList.FirstOrDefault(x => Request.Url.AbsolutePath.GetUrl().Equals(x.Path.GetUrl()));

                if (model.Seos == null || model.Seos.SeoId <= 0 || string.IsNullOrWhiteSpace(model.Seos.Path))
                {
                    return Redirect(Url.Action("NotFound", "Home"));
                }

                if (!Request.Url.AbsolutePath.Contains(model.Seos.Path))
                {
                    return RedirectPermanent(model.Seos.Path.GetUrl());
                }

                model.WebsiteTitle = model.Seos.MetaTitle;
                model.MetaTitle = model.Seos.MetaTitle;
                model.MetaDescription = model.Seos.MetaDescription;
                model.MetaKeywords = model.Seos.MetaKeyword;
                model.SeoHeader = model.Seos.H1Tag;

                if (!string.IsNullOrWhiteSpace(model.Seos.Name))
                    model.SeoName = model.Seos.Name;

                if (!string.IsNullOrWhiteSpace(model.Seos.Description))
                    model.SeoDescription = model.Seos.Description;

                if (!string.IsNullOrWhiteSpace(model.Seos.SeoFooter))
                    model.SeoFooter = model.Seos.SeoFooter;

                if (!string.IsNullOrWhiteSpace(model.Seos.ImagePath))
                    model.WebsiteImage = model.Seos.ImagePath.GetImageUrl();
            }

            return View(model);
        }

        /// <summary>
        /// Văn bản chia di sản thừa kế là Nhà - Đất
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DocumentDivisionInheritanceHouseLand(DocumentDivisionInheritanceHouseLandVM model)
        {
            if (model.DistrictId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "DistrictId",
                    Message = "Vui lòng chọn Quận/Huyện"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.WardId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "WardId",
                    Message = "Vui lòng chọn Phường/Xã"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.StreetId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "StreetId",
                    Message = "Vui lòng chọn Đường/Phố"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandTypeId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandTypeId",
                    Message = "Vui lòng chọn Loại đất"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LocationId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LocationId",
                    Message = "Vui lòng chọn Vị trí"
                }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrWhiteSpace(model.LandArea))
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Đất"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandArea.Trim().Length > 9)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Đất tối đa 9 ký tự"
                }, JsonRequestBehavior.AllowGet);
            }

            var landAreaMatch = Regex.Match(model.LandArea, @"^[0-9]+\.?([0-9]+)?$", RegexOptions.IgnoreCase);

            if (!landAreaMatch.Success)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandArea",
                    Message = "Vui lòng nhập Diện tích Đất hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrWhiteSpace(model.HouseArea))
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích sàn xây dựng"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.HouseArea.Trim().Length > 9)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích sàn xây dựng tối đa 9 ký tự"
                }, JsonRequestBehavior.AllowGet);
            }

            var houseAreaMatch = Regex.Match(model.HouseArea, @"^[0-9]+\.?([0-9]+)?$", RegexOptions.IgnoreCase);

            if (!houseAreaMatch.Success)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "HouseArea",
                    Message = "Vui lòng nhập Diện tích sàn xây dựng hợp lệ"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.Year <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "Year",
                    Message = "Vui lòng chọn Năm xây dựng"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.LandId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "LandId",
                    Message = "Vui lòng chọn Cấp công trình"
                }, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrWhiteSpace(model.SubmitAction))
            {
                model.LandArea = model.LandArea.Replace(",", ".");
                model.HouseArea = model.HouseArea.Replace(",", ".");

                Contracts contracts = await Contracts.Static_DocumentDivisionInheritanceHouseLand(double.Parse(model.HouseArea, System.Globalization.CultureInfo.InvariantCulture), double.Parse(model.LandArea, System.Globalization.CultureInfo.InvariantCulture), model.LandId, model.Year, model.LocationId);

                if (contracts != null && !string.IsNullOrWhiteSpace(contracts.ActionStatus) && contracts.ActionStatus.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('.row-ketqua').find('.gia-tri-nha-theo-khung-gia-nha-nuoc').first().html('{0:0,0}');$('.row-ketqua').find('.phi-cong-chung').first().html('{1:0,0}');$('.row-ketqua').removeClass('hidden');if($(window).scrollTop() > 150) $(\"html, body\").animate({{ scrollTop: $('.section-shadow').offset().top }});", contracts.AssetValue, contracts.NotarizationFee)))
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Completed = false,
                        Message = contracts.ActionMessage
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    Completed = true,
                    Data = "NoAction",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region VehicleSalesContract

        /// <summary>
        /// Hợp đồng mua bán xe
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> VehicleSalesContract()
        {
            CacheVM cacheVM = cacheHelper.Get<CacheVM>("CacheVM");

            if (cacheVM == null)
            {
                Task<HitCounter> hitCounterTask = HitCounter.Static_GetAsync();
                Task<Pages> pagesTask = Pages.Static_GetById("", ConstantHelper.DefaultPageId);
                Task<List<MenuItems>> menuItemsTask = MenuItems.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<Seos>> seosTask = Seos.Static_GetList(ConstantHelper.StatusIdActivated);
                Task<List<VehicleTypes>> vehicleTypesTask = VehicleTypes.Static_GetList();
                Task<List<VehicleRegistrationYears>> vehicleRegistrationYearsTask = VehicleRegistrationYears.Static_GetList();

                await Task.WhenAll(hitCounterTask, pagesTask, menuItemsTask, seosTask, vehicleTypesTask, vehicleRegistrationYearsTask);

                cacheVM = new CacheVM
                {
                    HitCounter = hitCounterTask.Result,
                    MenuItemsList = menuItemsTask.Result,
                    Pages = pagesTask.Result,
                    SeosList = seosTask.Result,
                    VehicleTypesList = vehicleTypesTask.Result,
                    VehicleRegistrationYearsList = vehicleRegistrationYearsTask.Result
                };
            }

            VehicleSalesContractVM model = new VehicleSalesContractVM
            {
                Pages = cacheVM.Pages,
                HitCounter = cacheVM.HitCounter,
                MenuItemsList = cacheVM.MenuItemsList,
                DaysList = new List<ObjectJsonVM>(),
                MonthsList = new List<ObjectJsonVM>(),
                VehicleTypesList = cacheVM.VehicleTypesList.IsAny() ? cacheVM.VehicleTypesList : new List<VehicleTypes>(),
                VehicleRegistrationYearsList = cacheVM.VehicleRegistrationYearsList.IsAny() ? cacheVM.VehicleRegistrationYearsList : new List<VehicleRegistrationYears>()
            };

            int year = 0;

            if (model.VehicleRegistrationYearsList.IsAny())
            {
                year = model.VehicleRegistrationYearsList[model.VehicleRegistrationYearsList.Count - 1].Value;
            }

            for (int index = 1; index < DateTime.DaysInMonth(year > 0 ? year : DateTime.Now.Year, DateTime.Now.Month); index++)
            {
                model.DaysList.Add(new ObjectJsonVM { Id = index, Name = index.ToString() });
            }

            for (int index = 1; index <= 12; index++)
            {
                model.MonthsList.Add(new ObjectJsonVM { Id = index, Name = index.ToString() });
            }

            if (cacheVM.SeosList.IsAny())
            {
                model.Seos = cacheVM.SeosList.FirstOrDefault(x => Request.Url.AbsolutePath.GetUrl().Equals(x.Path.GetUrl()));

                if (model.Seos == null || model.Seos.SeoId <= 0 || string.IsNullOrWhiteSpace(model.Seos.Path))
                {
                    return Redirect(Url.Action("NotFound", "Home"));
                }

                if (!Request.Url.AbsolutePath.Contains(model.Seos.Path))
                {
                    return RedirectPermanent(model.Seos.Path.GetUrl());
                }

                model.WebsiteTitle = model.Seos.MetaTitle;
                model.MetaTitle = model.Seos.MetaTitle;
                model.MetaDescription = model.Seos.MetaDescription;
                model.MetaKeywords = model.Seos.MetaKeyword;
                model.SeoHeader = model.Seos.H1Tag;

                if (!string.IsNullOrWhiteSpace(model.Seos.Name))
                    model.SeoName = model.Seos.Name;

                if (!string.IsNullOrWhiteSpace(model.Seos.Description))
                    model.SeoDescription = model.Seos.Description;

                if (!string.IsNullOrWhiteSpace(model.Seos.SeoFooter))
                    model.SeoFooter = model.Seos.SeoFooter;

                if (!string.IsNullOrWhiteSpace(model.Seos.ImagePath))
                    model.WebsiteImage = model.Seos.ImagePath.GetImageUrl();
            }

            return View(model);
        }

        /// <summary>
        /// Hợp đồng mua bán xe
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> VehicleSalesContract(VehicleSalesContractVM model)
        {
            if (model.VehicleTypeId <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "VehicleTypeId",
                    Message = "Vui lòng chọn Loại xe"
                }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrWhiteSpace(model.Price))
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "Price",
                    Message = "Vui lòng nhập Giá xe mới (VNĐ)"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.Day <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "Day",
                    Message = "Vui lòng chọn Ngày đăng ký"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.Month <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "Month",
                    Message = "Vui lòng chọn Tháng đăng ký"
                }, JsonRequestBehavior.AllowGet);
            }

            if (model.Year <= 0)
            {
                return Json(new
                {
                    Completed = false,
                    FieldValidationError = "Year",
                    Message = "Vui lòng chọn Năm đăng ký"
                }, JsonRequestBehavior.AllowGet);
            }

            if (!string.IsNullOrWhiteSpace(model.SubmitAction))
            {
                model.Price = model.Price.Replace(",", "").Replace(".", "");

                DateTime firstVehicleRegistrationDate = new DateTime(model.Year, model.Month, model.Day);

                Tuple<string, string, double> resultVar = await VehicleDepreciation.Static_VehicleSalesContract(double.Parse(model.Price), firstVehicleRegistrationDate);

                if (resultVar != null && !string.IsNullOrWhiteSpace(resultVar.Item1) && resultVar.Item1.Equals(ConstantHelper.ActionStatusSuccess))
                {
                    return Json(new
                    {
                        Completed = true,
                        Cb = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("$('.row-ketqua').find('.phi-cong-chung').first().html('{0:0,0}');$('.row-ketqua').removeClass('hidden');if($(window).scrollTop() > 150) $(\"html, body\").animate({{ scrollTop: $('.section-shadow').offset().top }});", resultVar.Item3)))
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        Completed = false,
                        Message = resultVar.Item2
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    Completed = true,
                    Data = "NoAction",
                }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

    }
}