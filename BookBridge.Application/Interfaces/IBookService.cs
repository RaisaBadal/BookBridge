﻿using BookBridge.Application.Models.Request;

namespace BookBridge.Application.Interfaces
{
    public interface IBookService:ICrud<BookModel,long>
    {
    }
}
