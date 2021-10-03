namespace SolutionTemplate.BlazorUI.Hosting.Models.DTO
{
    public abstract class DTOGPSModel : DTOModel
    {
        /// <summary>Широта</summary>
        public double Latitude { get; set; }

        /// <summary>Долгота</summary>
        public double Longitude { get; set; }
    }
}
