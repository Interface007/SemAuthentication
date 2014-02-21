// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpertsController.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ExpertsController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.WebSite.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    using Sem.Authentication.InAppIps;
    using Sem.Authentication.MvcHelper.WebSite.Models;
    using Sem.Authentication.WebApiHelper.InAppIps;

    public class ExpertsController : ApiController
    {
        private readonly List<Expert> model = new List<Expert>
            {
                new Expert { Id = 123, FullName = "Wolfgang Amadeus Mozart", Experience = 100 }, 
                new Expert { Id = 124, FullName = "Nicolo Paragini", Experience = 95 }, 
                new Expert { Id = 125, FullName = "Richard Wagner", Experience = 120 }, 
            };

        // GET api/<controller>
        public IEnumerable<Expert> Get()
        {
            return this.model;
        }

        // GET api/<controller>/5
        [LandmineWebApi(LandmineName = "accesslevel", ExpectedValue = "public", RequestArea = RequestArea.Header, Seconds = 10)]
        public Expert Get(int id)
        {
            return this.model.FirstOrDefault(x => x.Id == id);
        }

        // POST api/<controller>
        public void Post([FromBody]Expert value)
        {
            value.Id = this.model.Max(x => x.Id) + 1;
            this.model.Add(value);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]Expert value)
        {
            var item = this.model.FirstOrDefault(x => x.Id == value.Id);
            if (item == null)
            {
                return;
            }

            item.FullName = value.FullName;
            item.Experience = value.Experience;
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            this.model.Remove(this.model.FirstOrDefault(x => x.Id == id));
        }
    }
}