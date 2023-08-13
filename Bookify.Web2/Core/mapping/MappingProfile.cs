using AutoMapper;
using Bookify.Web2.Core.Models;
using Bookify.Web2.Core.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookify.Web2.Core.mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            //categories
            CreateMap<category, CategoryViewModel>();
            CreateMap<CreateViewModelCategory, category>();
            CreateMap<category, EditViewModelCategory>().ReverseMap();
            CreateMap<category, SelectListItem>().
                ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Text, opt => opt.MapFrom(src => src.Name));
            //authors
            CreateMap<Author, AuthorViewModel>();
            CreateMap<CreateViewModelAuthor, Author>();
            CreateMap<Author, EditViewModelAuthor>().ReverseMap();
            CreateMap<Author, SelectListItem>().
                ForMember(dst => dst.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Text, opt => opt.MapFrom(src => src.Name));


            //books
            CreateMap<BookFormViewModel, Book>().ReverseMap()
                .ForMember(dest => dest.categories, opt => opt.Ignore());
            /*          also this right  .ForMember(src => src.categories, opt => opt.Ignore());
            */

            CreateMap<Book, BookViewModel>().
                     ForMember(dst => dst.Author, opt => opt.MapFrom(src => src.Author!.Name)).
                     ForMember(dst => dst.Categories,
                    opt => opt.MapFrom(src => src.Categories.Select(c => c.Category!.Name).ToList()));
            CreateMap<BookCopy, BookCopiesModel> ().
                   ForMember(dst => dst.BookTitle, opt => opt.MapFrom(src => src.Book!.Title)).
                   ForMember(dst => dst.BookId, opt => opt.MapFrom(src => src.Book!.Id)).
                   ForMember(dst => dst.BookThumbnailUrl, opt => opt.MapFrom(src => src.Book!.ImageThumnabilUrl));

            CreateMap<BookCopy, CopiesFormViewModel>();


            CreateMap<ApplicationUser, ChangePasswordViewModelUser>().ReverseMap();
            
            CreateMap<ApplicationUser, UserViewModel>();
            CreateMap<UserFormViewModel, ApplicationUser>()
                .ForMember(dst => dst.NormalizedUserName, opt => opt.MapFrom(src => src.UserName.ToUpper()))
                .ForMember(dst => dst.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ReverseMap();

            CreateMap<Governorate, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));

            CreateMap<Area, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Name));

            //Subscribers
            CreateMap<Subscriber, SubscriberFormViewModel>().ReverseMap();

            /*    CreateMap<Subscriber, SubscriberSearchResultViewModel>()
                    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
    */
            /* CreateMap<Subscriber, SubscriberViewModel>()
                 .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                 .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area!.Name))
                 .ForMember(dest => dest.Governorate, opt => opt.MapFrom(src => src.Governorate!.Name));*/
            CreateMap<Subscriber, SubscriberSearchViewModel>().
                ForMember(dest=>dest.FullName,opt=>opt.MapFrom(src=>$"{src.FirstName} {src.LastName}"));

            CreateMap<Subscriber, SubscriberViewModel>().ForMember(
                src => src.FullName, opt => opt.MapFrom(src => $"{src.FirstName}{src.LastName}")).
                ForMember(src=>src.governorate,opt=>opt.MapFrom(src=>src.governorate!.Name)).
                ForMember(src=>src.area,opt=>opt.MapFrom(src=>src.area!.Name));

            CreateMap<Subscribtions, SubscribtionViewModel>();

            CreateMap<Rental, RentalViewModel>();
            
            CreateMap<RentalCopy, RentalCopyViewModel>();

        }
    }
}
