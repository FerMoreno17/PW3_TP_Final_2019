using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Models;
using PracticaEF.Data;
using PracticaEF.Servicios;

namespace Api.Controllers
{
    public class HistorialDonacionesController : ApiController
    {
        DonacionService don = new DonacionService();
        Entities1 ctx = new Entities1();

        // GET: api/HistorialDonaciones/5
        public List<DonacionApi> Get(int id)
        {
            return don.MisDonacionesId(id);
        }
    }
}

