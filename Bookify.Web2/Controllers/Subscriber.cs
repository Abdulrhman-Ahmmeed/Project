    using Bookify.Web2.Services;
using Hangfire;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using WhatsAppCloudApi;
using WhatsAppCloudApi.Services;

namespace Bookify.Web2.Controllers
    {

        public class Subscriber : Controller
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IImageService _imageService;
            private readonly IDataProtector _dataProtector;
            private readonly IWebHostEnvironment _webHost;
            private readonly IWhatsAppClient _whatsAppClient;
            private readonly IEmailSender _emailSender;
            private readonly IEmailBodyBuilder _BodyBuilder;

        public Subscriber(ApplicationDbContext context, IMapper mapper, IDataProtectionProvider dataprotector, IImageService imageService, IWebHostEnvironment webHost, IWhatsAppClient whatsAppClient, IEmailSender emailSender, IEmailBodyBuilder bodyBuilder)
        {
            _context = context;
            _mapper = mapper;
            _imageService = imageService;
            _webHost = webHost;
            _dataProtector = dataprotector.CreateProtector("MySecureKey");
            _whatsAppClient = whatsAppClient;
            _emailSender = emailSender;
            _BodyBuilder = bodyBuilder;
        }

        public async Task<IActionResult> Index()
        {

            /* var subscribers = await _context.subscribers.ToListAsync();
             var viewModel = _mapper.Map<IEnumerable<SubscriberViewModel>>(subscribers);
             return View(viewModel);*/



            

            return View();

        }

        [HttpGet]
        public IActionResult Create()
        {
                
            return View("Form",RenderViewModel());
        }
        
        [HttpGet]
        public IActionResult Details(string id)
        {
            var subscriberId = int.Parse(_dataProtector.Unprotect(id));
            var subscriber = _context.subscribers
                .Include(x=>x.governorate)
                .Include(y=>y.area)
                .Include(s=>s.subscribtions)
                .Include(r=>r.Rentals)
                .ThenInclude(c=>c.RentalsCopies)
                .SingleOrDefault(b => b.Id == subscriberId);
            if (subscriber is null)
                return NotFound();
            var viewModel = _mapper.Map<SubscriberViewModel>(subscriber);
            viewModel.key = id;
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> create(SubscriberFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                
                return View("Form", RenderViewModel(model));
            }
            var subscriber = _mapper.Map<Core.Models.Subscriber>(model);
          
                string imageName = $"{Guid.NewGuid()},{Path.GetExtension(model.Image.FileName)}";
                var (IsUploaded, errorMessage) = await _imageService.UploadAsync(model.Image, imageName, "/Images/subscribers", IsThumbnail: true);

                if (IsUploaded)
                {
                    // to save url of image
                    // not just name of image
                    subscriber.ImageUrl = $"/Images/subscribers/{imageName}";
                    // to save thumbUrl 
                    subscriber.ImageThumnabilUrl = $"/Images/subscribers/thumb/{imageName}";
                }
                else
                {
                    ModelState.AddModelError(nameof(Image), errorMessage!);
                    return View("Index", RenderViewModel(model));
                }
            
            subscriber.ImageUrl = $"/Images/subscribers/{imageName}";
            subscriber.ImageThumnabilUrl = $"/Images/subscribers/thumb/{imageName}";
            subscriber.CreatedOnByID = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            Subscribtions subscribtions = new()
            {
                CreatedOn = subscriber.CreatedOn,
                CreatedOnByID = subscriber.CreatedOnByID,
                StartDate = DateTime.Now,
                EndDate = DateTime.Today.AddYears(1)
                

            };
            subscriber.subscribtions.Add(subscribtions);
            _context.Add(subscriber);
            _context.SaveChanges();


            var placeHolder = new Dictionary<string, string>()
                {
                    {"imageUrl","https://res.cloudinary.com/devcreed/image/upload/v1668732314/icon-positive-vote-1_rdexez.svg" },
                    {"header",$"Welcome {model.FirstName}, thank for joining us!" },
                    {"body","Thanks for joining Bookify" },
                  

                };
            ///builder
            var body = _BodyBuilder.GetEmailBody(EmailTemplates.Notifiction, placeHolder);

            BackgroundJob.Enqueue(()=>_emailSender.SendEmailAsync(model.Email, "Welcome to Bookify", body));

            if (model.HasWhatsApp)
            {
              /*  var componants = new List<WhatsAppComponent>()
                {
                    new WhatsAppComponent
                    {
                        Type="body",
                        Parameters=new List<object>()
                        {
                            new WhatsAppTextParameter{Text="Abdulrhman"}
                        }
                    }
                };*/
                var Mobilenumber = _webHost.IsDevelopment() ? "01060768901" : model.MobileNumber;
                await _whatsAppClient.
                    SendMessage($"2{Mobilenumber}", WhatsAppLanguageCode.English_US, WhatsAppTemplates.welcome);


            }

            var subscriberId = _dataProtector.Protect(subscriber.Id.ToString());

            return RedirectToAction(nameof(Details),new { id = subscriberId });
        }
        
        [HttpGet]
        public IActionResult Edit(string Id)
        {
            var subscriberId =int.Parse(_dataProtector.Unprotect(Id));
            var subscriber = _context.subscribers.FirstOrDefault(x=>x.Id==subscriberId);
            if (subscriber is null)
            {
                return NotFound();
            }
            var model = _mapper.Map<SubscriberFormViewModel>(subscriber);
            model.ImageThumbnailUrl = subscriber.ImageThumnabilUrl;
            model.Key = Id;
            return View("Form", RenderViewModel(model));
        }
        [HttpPost]
/*        public async Task<IActionResult> Edit(SubscriberFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", RenderViewModel(model));
            }
            var subscriber = _mapper.Map<Core.Models.Subscriber>(model);

            string imageName = $"{Guid.NewGuid()},{Path.GetExtension(model.Image.FileName)}";
            var (IsUploaded, errorMessage) = await _imageService.UploadAsync(model.Image, imageName, "/Images/subscribers", IsThumbnail: true);

            if (IsUploaded)
            {
                // to save url of image
                // not just name of image
                subscriber.ImageUrl = $"/Images/subscribers/{imageName}";
                // to save thumbUrl 
                subscriber.ImageThumnabilUrl = $"/Images/subscribers/thumb/{imageName}";
            }
            else
            {
                ModelState.AddModelError(nameof(Image), errorMessage!);
                return View("Index", RenderViewModel(model));
            }

            subscriber.ImageUrl = $"/Images/subscribers/{imageName}";
            subscriber.ImageThumnabilUrl = $"/Images/subscribers/thumb/{imageName}";
            subscriber.CreatedOnByID = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            subscriber = _mapper.Map(model, subscriber);
            subscriber.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            subscriber.LastUpdated = DateTime.Now;
           
            subscriber.Id = int.Parse(_dataProtector.Unprotect(model.Key!));

            _context.Add(subscriber);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = model.Key! });



        }
*/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubscriberFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", RenderViewModel(model));

            var subscriberId = int.Parse(_dataProtector.Unprotect(model.Key));

            var subscriber = _context.subscribers.Find(subscriberId);

            if (subscriber is null)
                return NotFound();

            if (model.Image is not null)
            {
                if (!string.IsNullOrEmpty(subscriber.ImageUrl))
                    _imageService.Delete(subscriber.ImageUrl, subscriber.ImageThumnabilUrl);

                var imageName = $"{Guid.NewGuid()}{Path.GetExtension(model.Image.FileName)}";
                var imagePath = "/Images/subscribers";

                var (IsUploaded, errorMessage) = await _imageService.UploadAsync(model.Image, imageName, imagePath, IsThumbnail: true);

                if (!IsUploaded)
                {
                    ModelState.AddModelError("Image", errorMessage!);
                    return View("Form", RenderViewModel(model));
                }

                model.ImageUrl = $"{imagePath}/{imageName}";
                model.ImageThumbnailUrl = $"{imagePath}/thumb/{imageName}";
            }

            else if (!string.IsNullOrEmpty(subscriber.ImageUrl))
            {
                model.ImageUrl = subscriber.ImageUrl;
                model.ImageThumbnailUrl = subscriber.ImageThumnabilUrl;
            }

            subscriber = _mapper.Map(model, subscriber);
            subscriber.LastUpdatedById = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            subscriber.LastUpdated = DateTime.Now;

            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = model.Key });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(SearchFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var subscriber = _context.subscribers
                            .SingleOrDefault(s =>
                                    s.Email == model.Value
                                || s.NationalId == model.Value
                                || s.MobileNumber == model.Value);
            var viewModel = _mapper.Map<SubscriberSearchViewModel>(subscriber);
            if (subscriber is not null)
            {
                viewModel.Key = _dataProtector.Protect(subscriber.Id.ToString());

            }


            return PartialView("_Result", viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> RenewSubcriber(string sKey)
        {
            var subscriberId = int.Parse(_dataProtector.Unprotect(sKey));
            var subscriber = _context.subscribers.Include(s=>s.subscribtions).FirstOrDefault(x => x.Id == subscriberId);
            if (subscriber is null)
                 return NotFound();
            
            if (subscriber.IsBlackedList)
                 return BadRequest();

           var LastSubscribtions = subscriber.subscribtions.Last();
            var StartDate = LastSubscribtions.EndDate < DateTime.Now ? DateTime.Now : LastSubscribtions.EndDate.AddDays(1);


            Subscribtions subscribtions = new()
            {
                CreatedOnByID = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                CreatedOn = DateTime.Now,
                StartDate = StartDate,
                EndDate=StartDate.AddYears(1)
            };

            subscriber.subscribtions.Add(subscribtions);
            _context.SaveChanges();


            var placeHolder = new Dictionary<string, string>()
                {
                    {"imageUrl","https://res.cloudinary.com/devcreed/image/upload/v1668732314/icon-positive-vote-1_rdexez.svg" },
                    {"header",$"  thanks {subscriber.FirstName} for  Trusting us!" },
                    {"body","Your subscribtion renewed successfully" },


                };
            ///builder
            var body = _BodyBuilder.GetEmailBody(EmailTemplates.Notifiction, placeHolder);

            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(subscriber.Email, "Welcome to Bookify", body));

            if (subscriber.HasWhatsApp)
            {
                /*  var componants = new List<WhatsAppComponent>()
                  {
                      new WhatsAppComponent
                      {
                          Type="body",
                          Parameters=new List<object>()
                          {
                              new WhatsAppTextParameter{Text="Abdulrhman"}
                          }
                      }
                  };*/
                var Mobilenumber = _webHost.IsDevelopment() ? "01060768901" : subscriber.MobileNumber;
                await _whatsAppClient.
                    SendMessage($"2{Mobilenumber}", WhatsAppLanguageCode.English_US, WhatsAppTemplates.welcome);

            }



            var viewModel = _mapper.Map<SubscribtionViewModel>(subscribtions);
            return PartialView("_SubscribtionRow",viewModel);
        }
        public IActionResult den()
        {
            RecurringJob.AddOrUpdate(() => ExpiredAlertMessage(), Cron.Daily);

            return Ok();
        }

        public async Task ExpiredAlertMessage()
        {
            var subscribtions =  _context.subscribers
                                        .Include(s => s.subscribtions)
                                        .Where(s =>!s.IsBlackedList&& s.subscribtions.OrderByDescending(x => x.EndDate).First().EndDate == DateTime.Today.AddDays(5));
            foreach (var subscribtion in subscribtions)
            {
                var endDate =subscribtion.subscribtions.Last().EndDate.ToString("d MMM, yyyy");
                var placeHolder = new Dictionary<string, string>()
                {
                    {"imageUrl","https://res.cloudinary.com/devcreed/image/upload/v1668732314/icon-positive-vote-1_rdexez.svg" },
                    {"header",$"Hello {subscribtion.FirstName}, " },
                    {"body",$"Your subscribtion end date is {endDate} !! " },
                };
                ///builder
                var body = _BodyBuilder.GetEmailBody(EmailTemplates.Notifiction, placeHolder);
                await _emailSender
                    .SendEmailAsync(subscribtion.Email, "Welcome to Bookify", body);
                if (subscribtion.HasWhatsApp)
                {
                    var componants = new List<WhatsAppComponent>()
                      {
                          new WhatsAppComponent
                          {
                              Type="body",
                              Parameters=new List<object>()
                              {
                                  new WhatsAppTextParameter{Text=subscribtion.FirstName},
                                  new WhatsAppTextParameter{Text=endDate}
                              }
                          }
                     };
                    var mobileNumber = _webHost.IsDevelopment() ? "01060768901" : subscribtion.MobileNumber;
                    await _whatsAppClient
                        .SendMessage($"2{mobileNumber}", WhatsAppLanguageCode.English, WhatsAppTemplates.welcome, componants);



                }
            }
        }

        private SubscriberFormViewModel RenderViewModel(SubscriberFormViewModel? model = null)
        {
                SubscriberFormViewModel viewModel = model is null ? new SubscriberFormViewModel() : model;
                var governorates = _context.governorates.Where(g => !g.IsDeleted).OrderBy(g => g.Name).ToList();
                viewModel.Governorates = _mapper.Map<IEnumerable<SelectListItem>>(governorates);
                if (model?.GovernorateId > 0)
                {
                    var areas = _context.areas
                        .Where(a => a.GovernorateId == model.GovernorateId && !a.IsDeleted)
                        .OrderBy(a => a.Name)
                        .ToList();
                    viewModel.Areas = _mapper.Map<IEnumerable<SelectListItem>>(areas);
                }
                return viewModel;
            }

            [HttpGet]
            public IActionResult GetAreas(int governorateId)
            {
                var areas = _context.areas
                    .Where(a => a.GovernorateId == governorateId&&!a.IsDeleted).Select(g => new SelectListItem
                    {
                        //هنا دا ليه علاقة بالjs 
                        //هناك الفليو محتاجة تكون مبعوته اي دي وال تكست تكست
                        Value = g.Id.ToString(),
                        Text = g.Name
                    }).OrderBy(b => b.Text)
                    .ToList();
    
                return Ok(_mapper.Map<IEnumerable<SelectListItem>>(areas));
            }

        public IActionResult AllowNationalId(SubscriberFormViewModel model)
        {
            int subscriberId = 0;
            if (!string.IsNullOrEmpty(model.Key))
            {
                subscriberId = int.Parse(_dataProtector.Unprotect(model.Key));
            }
            var subscriber = _context.subscribers.SingleOrDefault(s => s.NationalId == model.NationalId);
            var IsAllowed = subscriber is null || subscriber.Id.Equals(subscriberId);
            return Json(IsAllowed);
        }
        public IActionResult AllowMobileNumber(SubscriberFormViewModel model)
        {
            int subscriberId = 0;
            if (!string.IsNullOrEmpty(model.Key))
            {
                subscriberId = int.Parse(_dataProtector.Unprotect(model.Key));
            }
            var subscriber = _context.subscribers.SingleOrDefault(s => s.MobileNumber == model.MobileNumber);
            var IsAllowed = subscriber is null || subscriber.Id.Equals(subscriberId);
            return Json(IsAllowed);
        }

        public IActionResult AllowEmail(SubscriberFormViewModel model)
        {
            int subscriberId = 0;
            if (!string.IsNullOrEmpty(model.Key))
            {
                subscriberId = int.Parse(_dataProtector.Unprotect(model.Key));
            }
            var subscriber = _context.subscribers.SingleOrDefault(s => s.Email == model.Email);
            var IsAllowed = subscriber is null || subscriber.Id.Equals(subscriberId);
            return Json(IsAllowed);
        }






    }
}
