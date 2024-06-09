using AutoMapper;
using BookBridge.Application.Models.Request;
using BookBridge.Domain.Entities;

namespace BookBridge.Application.Mapper
{
    public class AutoMapper:Profile
    {
        public AutoMapper()
        {
            CreateMap<Author, AuthorModel>().ReverseMap();
            CreateMap<Book, BookModel>().ReverseMap();
            CreateMap<BookCategory, BookCategoryModel>().ReverseMap();
            CreateMap<BorrowRecord, BorrowRecordModel>().ReverseMap();
            CreateMap<Notification, NotificationModel>().ReverseMap();
            CreateMap<Review, ReviewModel>().ReverseMap();
            CreateMap<WishlistItem, WishlistItemModel>().ReverseMap();
            CreateMap<Wishlist, WishlistModel>().ReverseMap();
            CreateMap<User,UserModel>().ReverseMap();
        }
    }
}
