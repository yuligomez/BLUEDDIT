using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLogWebApi.Models;
using ServerLogLogic;
using ServerLogLogicInterface;
using ServerLogWebApi.Helpers;

namespace ServerLogWebApi.Controllers
{
    [Route("v1/api/logs")]
    [ApiController]
    public class LogController : Controller
    {
        private readonly IServerLogLogic logLogic;
        private readonly IHttpContextAccessor httpContextAccessor;

        public LogController(IHttpContextAccessor httpContextAccessor)
        {
            logLogic = new LogLogic();
            this.httpContextAccessor = httpContextAccessor;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<Log>>> GetAllLogs([FromQuery] int page = 1, int pageSize = 15)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest();
            }
            
            PaginatedResponse<Log> logsPaginatedResponse =  await logLogic.GetAllLogs( page, pageSize);
            if (logsPaginatedResponse == null)
            {
                return NoContent();
            }
            else
            {
                string route = httpContextAccessor.HttpContext.Request.Host.Value +
                               httpContextAccessor.HttpContext.Request.Path;
                WebPaginatedResponse<Log> response =
                    WebPaginationHelper<Log>.GenerateWebPaginatedResponse(logsPaginatedResponse, page, pageSize, route);
                return Ok(response);
            }
            
        }
        
      
        [Route("users")]
        [HttpGet]
        public async Task<ActionResult<List<Log>>> GetFilteredUsername([FromQuery] FilterUsernameLogModel model, int page = 1, int pageSize = 15)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(model.Username))
            {
                return BadRequest(new ErrorModel("El nombre de usuario no puede estar vacío"));
            }
            else
            {
                PaginatedResponse<Log> logsPaginatedResponse =  await logLogic.GetLogsByUsernameAsync(model.Username, page, pageSize);
                if (logsPaginatedResponse == null)
                {
                    return NoContent();
                }
                else
                {
                    string route = httpContextAccessor.HttpContext.Request.Host.Value +
                           httpContextAccessor.HttpContext.Request.Path;
                    WebPaginatedResponse<Log> response =
                        WebPaginationHelper<Log>.GenerateWebPaginatedResponse(logsPaginatedResponse, page, pageSize, route);
                    return Ok(response);
                }
            }
        }
        
        [Route("types")]
        [HttpGet]
        public async Task<ActionResult<List<Log>>> GetFilteredTypeObject([FromQuery] FilterObjectTypeLogsModel model, int page = 1, int pageSize = 15)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest();
            }
            if (string.IsNullOrWhiteSpace(model.ObjectType))
            {
                return BadRequest(new ErrorModel("El campo no puede estar vacío"));
            }
            else
            {
                PaginatedResponse<Log> logsPaginatedResponse = await logLogic.GetLogsByTypeAsync(model.ObjectType, page, pageSize);
                if (logsPaginatedResponse == null)
                {
                    return NoContent();
                }
                else
                {
                    string route = httpContextAccessor.HttpContext.Request.Host.Value +
                           httpContextAccessor.HttpContext.Request.Path;
                    WebPaginatedResponse<Log> response =
                        WebPaginationHelper<Log>.GenerateWebPaginatedResponse(logsPaginatedResponse, page, pageSize, route);
                    return Ok(response);
                }
            } 
        }

        [Route("dates")]
        [HttpGet]
        public async Task<ActionResult<List<Log>>> GetFilteredDates([FromQuery] FilterDateLogModel model, int page = 1, int pageSize = 15)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest();
            }
            if (model.StartDate.Equals("") || model.EndDate.Equals(""))
            {
                return BadRequest(new ErrorModel("Las fechas no pueden estar vacías"));
            }
            else
            {
                PaginatedResponse<Log> logsPaginatedResponse = await logLogic.GetLogsByDateRangeAsync(model.StartDate, model.EndDate, page, pageSize);
                if (logsPaginatedResponse == null)
                {
                    return NoContent();
                }
                else
                {
                    string route = httpContextAccessor.HttpContext.Request.Host.Value +
                           httpContextAccessor.HttpContext.Request.Path;
                    WebPaginatedResponse<Log> response =
                        WebPaginationHelper<Log>.GenerateWebPaginatedResponse(logsPaginatedResponse, page, pageSize, route);
                    return Ok(response);
                }
            }
        }

        [Route("entityTypes")]
        [HttpGet]
        public async Task<ActionResult<List<Log>>> GetFilteredEntityType([FromQuery] FilterEntityTypeLogModel model, int page = 1, int pageSize = 15)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest();
            }
            if (string.IsNullOrWhiteSpace(model.LogType))
            {
                return BadRequest(new ErrorModel("El campo type no puede estar vacío"));
            }
            else
            {
                PaginatedResponse<Log> logsPaginatedResponse = await logLogic.GetEntityTypeLogsAsync(model.LogType, page, pageSize);
                if (logsPaginatedResponse == null)
                {
                    return NoContent();
                }
                else
                {
                    string route = httpContextAccessor.HttpContext.Request.Host.Value +
                           httpContextAccessor.HttpContext.Request.Path;
                    WebPaginatedResponse<Log> response =
                        WebPaginationHelper<Log>.GenerateWebPaginatedResponse(logsPaginatedResponse, page, pageSize, route);
                    return Ok(response);
                }
            }
        }
    }
}
