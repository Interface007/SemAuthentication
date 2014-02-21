// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SampleModel.cs" company="Sven Erik Matzen">
//   (c) 2013 Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the SampleModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.Authentication.MvcHelper.WebSite.Models
{
    public class Expert
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public int Experience { get; set; }

        public string LandmineValue { get; set; }
    }
}