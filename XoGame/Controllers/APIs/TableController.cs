using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using XoGame.Business;
using XoGame.Models;
using XoGame.Repositories;

namespace XoGame.Controllers.APIs
{
    public class TableController : ApiController
    {
        private readonly TableBusinessModel _tableBusinessModel;

        public TableController()
        {
            _tableBusinessModel = new TableBusinessModel();
        }

        [Route("api/Table/GetAll")]
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            try
            {
                return
                Ok(
                    _tableBusinessModel.GetCurrentTables()
                    );
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [Route("api/Table/AddTable")]
        [HttpPost]
        public IHttpActionResult AddTable(Table table)
        {
            try
            {
                return Ok(_tableBusinessModel.AddTable(table));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
