using Elegium.Models;
using Elegium.Models.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Dtos.ProjectDtos
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Detail { get; set; }
        public DateTime CreatedDateTime { get; set; }
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
        public Currency Currency { get; set; }
        public string Color { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string ContactFax { get; set; }
        public string UserId { get; set; }
        public bool IsSaved { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsSavedPartner { get; set; }
        public bool IsFavoritePartner { get; set; }
        public bool IsHired { get; set; }
        public bool IsStarted { get; set; }
        public bool IsFinished { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public string IsItMe { get; set; }
        public bool IsVoteFundingApplied { get; set; }
        public int OnboardPercentage { get; set; }
        public string CompletedVsInProgress { get; set; }
        public bool OnBoardingCompleted { get; set; }
        public string ProjectStatus { get; set; }
    }
}
