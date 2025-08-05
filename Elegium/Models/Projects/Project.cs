using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models.Projects
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public string ProjectFunctions { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedDateTime { get; set; }

        #region Production Data
        public ProductionType ProductionType { get; set; }
        public int ProductionTypeId { get; set; }
        public string ProductionLength { get; set; }
        public int? ProductionLengthMM { get; set; }
        public int? ProductionLengthSS { get; set; }
        public string ProductionRecordingMethod { get; set; }
        public string ProductionAspectRatio { get; set; }
        public string ProductionMode { get; set; }
        public string ProductionColor { get; set; }
        public Language ProductionLanguage { get; set; }
        public int ProductionLanguageId { get; set; }
        #endregion Production Data

        public Currency Currency { get; set; }
        public int CurrencyId { get; set; }
        public byte[] Logo { get; set; }
        public byte[] BackgroundImage { get; set; }
        public string Color { get; set; }
        #region Contact Details
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactFax { get; set; }
        #endregion Contact Details
        public bool isVisible { get; set; }
        public string VisibilityArea { get; set; }
        public bool IsStarted { get; set; }
        public bool IsFinished { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }

        //Link to the application user
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        public string BgColor { get; set; } = "#fff";
        public double? BgOpacity { get; set; } = 0;
        public int? OnBoardingPercentage { get; set; }
        public bool OnBoardingCompleted { get; set; }
        public bool DarkMode { get; set; }
        public bool GlassMode { get; set; }
        public bool CinematicMode { get; set; }

        public bool Deleted { get; set; }

    }
}
