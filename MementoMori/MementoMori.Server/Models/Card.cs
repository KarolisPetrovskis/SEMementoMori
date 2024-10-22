namespace MementoMori.Server
{

    public class Card
    {
        public required Guid Id { get; set; }

        public required string Question { get; set; }

        public string Description { get; set; }

        public required string Answer { get; set; }

        public int Interval { get; set; } = 1;    // Number of days until next review

        public int Repetitions { get; set; } = 0;  // Number of times reviewed

        public double EaseFactor { get; set; } = 2.5; // The factor that adjusts the interval

        public DateTime LastReviewed { get; set; }

        public void UpdateCard(int quality)
        {
            quality = Math.Max(0, Math.Min(quality, 5));

            if (quality >= 3) // remembered
            {
                if (Repetitions == 0)
                {
                    Interval = 1;
                }
                else if (Repetitions == 1)
                {
                    Interval = 6;
                }
                else
                {
                    Interval = (int)Math.Round(Interval * EaseFactor);
                }

                EaseFactor += 0.1 - (5 - quality) * (0.08 + (5 - quality) * 0.02);
                EaseFactor = Math.Max(1.3, EaseFactor); 

                Repetitions++;
            }
            else // forgot
            {
                Repetitions = 0;
                Interval = 1;
            }

            LastReviewed = DateTime.Now;
        }

        public bool IsDueForReview()
        {
            return DateTime.Now >= LastReviewed.AddDays(Interval);
        }   
    }
}

