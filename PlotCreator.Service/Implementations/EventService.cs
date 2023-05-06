using PlotCreator.DAL.Interfaces;
using PlotCreator.DAL.Repositories;
using PlotCreator.Domain.Entity;
using PlotCreator.Domain.Enum;
using PlotCreator.Domain.Response.Implementations;
using PlotCreator.Domain.Response.Interfaces;
using PlotCreator.Domain.ViewModels;
using PlotCreator.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlotCreator.Service.Implementations
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<IBaseResponse<EventViewModel>> CreateEvent(EventViewModel model)
        {
            var baseResponse = new BaseResponse<EventViewModel>();
            try
            {
                var Event = new Event()
                {
                    BookId = model.Book!.Id,
                    Title = model.Title,
                    Description = model.Description,
                    Beginning = model.Beginning,
                    Ending = model.Ending,
                    ChekhovsGun = model.ChekhovsGun,
                    IsHidden = model.IsHidden,
                    Icon = model.Icon,
                    Colour = model.Colour,
                };
                await _eventRepository.Add(Event);
                baseResponse.Data = model;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<EventViewModel>()
                {
                    Description = $"[EventService | CreateEvent]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<bool>> DeleteEvent(int id)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                var episode = await _eventRepository.GetOne(id);
                if (episode == null)
                {
                    baseResponse.Description = "Событие не найдено";
                    baseResponse.StatusCode = StatusCode.NotFound;
                    return baseResponse;
                }
                await _eventRepository.Delete(episode);
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[EventService | DeleteEvent]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<EventViewModel>> EditEvent(EventViewModel model)
        {
            var baseResponse = new BaseResponse<EventViewModel>();
            try
            {
                var Event = await _eventRepository.GetOne(model.Id);
                if(Event == null)
                {
                    baseResponse.Description = "Событие не найдено";
                    baseResponse.StatusCode = StatusCode.NotFound;
                    return baseResponse;
                }
                Event.BookId = model.Book!.Id;
                Event.Title = model.Title;
                Event.Description = model.Description;
                Event.Beginning = model.Beginning;
                Event.Ending = model.Ending;
                Event.ChekhovsGun = model.ChekhovsGun;
                Event.IsHidden = model.IsHidden;
                Event.Icon = model.Icon;
                Event.Colour = model.Colour;

                await _eventRepository.Update(Event);
                model.Book = Event.Book;
                model.Book!.User = Event.Book!.User;

                baseResponse.Data = model;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<EventViewModel>()
                {
                    Description = $"[EventService | EditEvent]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<EventViewModel>>> GetAllEvents(int userId)
        {
            var baseResponse = new BaseResponse<IEnumerable<EventViewModel>>();
            try
            {
                var Events = await _eventRepository.GetAllByUserId(userId);
                List<EventViewModel> EventModels = new List<EventViewModel>();
                foreach (var Event in Events)
                {
                    var model = new EventViewModel()
                    {
                        Id = Event.Id,
                        Book = Event.Book,
                        Title = Event.Title,
                        Description = Event.Description,
                        Beginning = Event.Beginning,
                        Ending = Event.Ending,
                        ChekhovsGun = Event.ChekhovsGun,
                        IsHidden = Event.IsHidden,
                        Icon = Event.Icon,
                        Colour = Event.Colour,
                    };
                    EventModels.Add(model);
                }
                baseResponse.Data = EventModels;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<EventViewModel>>()
                {
                    Description = $"[EventService | GetAllEvents]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<IEnumerable<EventViewModel>>> GetBookEvents(int bookId)
        {
            var baseResponse = new BaseResponse<IEnumerable<EventViewModel>>();
            try
            {
                var Events = await _eventRepository.GetAllByAnotherEntityId(bookId);
                List<EventViewModel> EventModels = new List<EventViewModel>();
                foreach (var Event in Events)
                {
                    var model = new EventViewModel()
                    {
                        Id = Event.Id,
                        Book = Event.Book,
                        Title = Event.Title,
                        Description = Event.Description,
                        Beginning = Event.Beginning,
                        Ending = Event.Ending,
                        ChekhovsGun = Event.ChekhovsGun,
                        IsHidden = Event.IsHidden,
                        Icon = Event.Icon,
                        Colour = Event.Colour,
                    };
                    EventModels.Add(model);
                }
                baseResponse.Data = EventModels;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<EventViewModel>>()
                {
                    Description = $"[EventService | GetBookEvents]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<EventViewModel>> GetEmptyViewModel(int bookId)
        {
            var baseResponse = new BaseResponse<EventViewModel>();
            try
            {
                var book = await _eventRepository.GetEventBook(bookId);
                var emptyModel = new EventViewModel()
                {
                    Book = book,
                };
                baseResponse.Data = emptyModel;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<EventViewModel>()
                {
                    Description = $"[EventService | GetEmptyViewModel]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<IBaseResponse<EventViewModel>> GetEvent(int EventId)
        {
            var baseResponse = new BaseResponse<EventViewModel>();
            try
            {
                var Event = await _eventRepository.GetOne(EventId);
                if (Event == null)
                {
                    baseResponse.Description = "Событие не найдено";
                    baseResponse.StatusCode = StatusCode.NotFound;
                    return baseResponse;
                }
                var data = new EventViewModel()
                {
                    Id = Event.Id,
                    Book = Event.Book,
                    Title = Event.Title,
                    Description = Event.Description,
                    Beginning = Event.Beginning,
                    Ending = Event.Ending,
                    ChekhovsGun = Event.ChekhovsGun,
                    IsHidden = Event.IsHidden,
                    Icon = Event.Icon,
                    Colour = Event.Colour,
                };
                baseResponse.Data = data;
                baseResponse.StatusCode = StatusCode.Ok;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<EventViewModel>()
                {
                    Description = $"[EventService | GetEvent]: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public async Task<int> GetUserId(int contentId)
        {
            var Event = await _eventRepository.GetOne(contentId);
            return Event.Book!.User!.Id;
        }
    }
}
