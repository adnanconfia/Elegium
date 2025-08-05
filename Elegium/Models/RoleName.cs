using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elegium.Models
{
    public static class RoleName
    {
        public const string Admin = "Administrator";
        public const string User = "Member";
    }

    public class NotificationKind
    {
        public const string Follow = "Follow";
        public const string FollowBack = "FollowBack";
        public const string HireRequestSent = "HireRequestSent";
        public const string MentionedComment = "MentionedComment";
        public const string ResourceRequestSent = "ResourceRequestSent";
        public const string ResourceRequestRejected = "ResourceRequestRejected";
        public const string ResourceRequestApproved = "ResourceRequestApproved";
        public const string FundingRequestReceived = "FundingRequestReceived";
        public const string FundingRequestApproved = "FundingRequestApproved";
        public const string FundingRequestRejected = "FundingRequestRejected";

        public const string ProjectPartnerRequestRejected = "ProjectPartnerRequestRejected";
        public const string ProjectPartnerRequestAppproved = "ProjectPartnerRequestAppproved";
        public const string ProjectPartnerRequestRequested = "ProjectPartnerRequestRequested";
    }
}
