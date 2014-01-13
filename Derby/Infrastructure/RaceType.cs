
using System.ComponentModel.DataAnnotations;

namespace Derby.Infrastructure
{
    public enum RaceType
    {
        [Display(Name = "Pinewood Derby")]
        PinewoodDerby,
        [Display(Name = "Raingutter Regatta")]
        RaingutterRegatta
    }
}