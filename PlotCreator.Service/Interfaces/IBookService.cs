﻿using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Service.Interfaces
{
    public interface IBookService
    {
        Task<IBaseResponse<IEnumerable<Book>>> GetBooks(int userId);
        Task<IBaseResponse<BookViewModel>> GetViewModel();
        Task<IBaseResponse<BookViewModel>> GetBook(int id);
        Task<IBaseResponse<BookViewModel>> CreateBook(BookViewModel bookViewModel);
        Task<IBaseResponse<Book>> EditBook (int? id, BookViewModel bookViewModel);
        Task<IBaseResponse<bool>> DeleteBook (int id);
        Task<int> GetUserId(int bookId);
    }
}
