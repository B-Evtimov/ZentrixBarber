namespace ZentrixBarber.Data.Models
{
    public class BarberTreatment
    {
        public int BarberId { get; set; }
        public int TreatmentId { get; set; }

        public virtual Barber Barber { get; set; } = null!;
        public virtual Treatment Treatment { get; set; } = null!;

    }
}
