myApp.config(function ($stateProvider, $urlRouterProvider, $locationProvider) {
  
    $urlRouterProvider.otherwise("/overview");
    var pId = window.location.href.charAt(window.location.href.indexOf('#') + 1) == '/' ? undefined : window.location.href.charAt(window.location.href.indexOf('#') + 1);
    $locationProvider.hashPrefix('');
   
    var dashboard = {
        name: 'dashboard',
        url: ':id/dashboard',
        templateUrl: '/ProjectManagement/_dashboard',
        controller: 'ProjectDashboardCtrl',
        data: {
            label: 'Dashboard' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams) {
                console.log($stateParams.id, 'bilal saeed');
            }
        }
    }

    var overview = {
        name: 'overview',
        url: '/overview',
        templateUrl: '/Home/overview',
        data: {
            label: 'Home Page' //label to show in breadcrumbs
        }
    }

    var admindashboard = {
        name: 'admindashboard',
        url: '/admindashboard',
        templateUrl: '/AdminDashboard/Index',
        data: {
            label: 'Admin Dashboard' //label to show in breadcrumbs
        }
    }

    var paymentgateway = {
        name: 'paymentgateway',
        url: '/paymentgateway',
        templateUrl: '/PaymentGateway/Index',
        data: {
            label: 'Payment Gateway' //label to show in breadcrumbs
        }
    }

    var notificationsettings = {
        name: 'notificationsettings',
        url: '/notificationsettings',
        templateUrl: '/AdminDashboard/NotificationSettings',
        data: {
            label: 'Notification Settings' //label to show in breadcrumbs
        }
    }

    var publicprojects = {
        name: 'publicprojects',
        url: '/publicprojects',
        templateUrl: '/PublicProjects/Index',
        data: {
            label: 'Public Projects' //label to show in breadcrumbs
        }
    }

    var myprojects = {
        name: 'myprojects',
        url: '/myprojects',
        templateUrl: '/Projects/Index',
        data: {
            label: 'My Projects' //label to show in breadcrumbs
        }
    }

    var createnewproject = {
        name: 'createnewproject',
        url: '/createnewproject/:newProject',
        templateUrl: '/Projects/Index',
        data: {
            label: 'My Projects' //label to show in breadcrumbs
        },
        params: {
            newProject: { squash: true, value: null }
        }
    }

    var disputestome = {
        name: 'disputestome',
        url: '/disputestome',
        templateUrl: '/ProjectDisputes/Index',
        data: {
            label: 'Disputes to me' //label to show in breadcrumbs
        }
    }

    var mydisputes = {
        name: 'mydisputes',
        url: '/mydisputes',
        templateUrl: '/ProjectDisputes/My',
        data: {
            label: 'My disputes' //label to show in breadcrumbs
        }
    }

    var professionals = {
        name: 'professionals',
        url: '/professionals',
        controller: 'ProfessionalController',
        templateUrl: '/Professionals/Index',
        data: {
            label: 'Professionals' //label to show in breadcrumbs
        }
    }

    var profile = {
        name: 'profile',
        url: '/profile',
        templateUrl: '/Profile/Index',
        data: {
            label: 'Profile' //label to show in breadcrumbs
        }
    }


    var professionaldetails = {
        name: 'professionaldetails',
        url: '/professionaldetails/:userId',
        templateUrl: function ($location) {
            return '/ProfessionalDetail/Index?userId=' + $location.userId || '';
        },
        controller: 'professionalDetailCtrl',
        data: {
            label: 'Professional details' //label to show in breadcrumbs
        }
    }

    var publicprojectdetail = {
        name: 'publicprojectdetail',
        url: '/publicprojectdetail/:projectId',
        templateUrl: '/PublicProjectDetail/Index',
        data: {
            label: 'Public project detail' //label to show in breadcrumbs
        }
    }

    var hirerequests = {
        name: 'hirerequests',
        url: '/hirerequests',
        templateUrl: '/ProfessionalHireRequests/Index',
        data: {
            label: 'Hire requests' //label to show in breadcrumbs
        }
    }

    var offers = {
        name: 'offers',
        url: '/offers',
        templateUrl: '/ProfessionalHireRequests/My',
        data: {
            label: 'My offers' //label to show in breadcrumbs
        }
    }

    var projectpartner = {
        name: 'projectpartner',
        url: '/projectpartner',
        templateUrl: '/ProjectPartners/Index',
        data: {
            label: 'Project partners' //label to show in breadcrumbs
        }
    }

    var projectpartnerrequests = {
        name: 'projectpartnerrequests',
        url: '/projectpartnerrequests',
        templateUrl: '/ProjectPartners/ProjectPartnerRequests',
        data: {
            label: 'Project partners requests' //label to show in breadcrumbs
        }
    }

    var offerfunding = {
        name: 'offerfunding',
        url: '/offerfunding',
        templateUrl: '/FundingAndFP/Offer',
        data: {
            label: 'Offer funding' //label to show in breadcrumbs
        }
    }

    var lookingforfunding = {
        name: 'lookingforfunding',
        url: '/lookingforfunding',
        templateUrl: '/FundingAndFP/Index',
        data: {
            label: 'Looking for funding' //label to show in breadcrumbs
        }
    }

    var fundingRequests = {
        name: 'fundingrequests',
        url: '/fundingRequests/',
        templateUrl: '/FundingAndFP/FundingRequests',
        data: {
            label: 'Funding requests' //label to show in breadcrumbs
        }
    }

    var offerresource = {
        name: 'offerresource',
        url: '/offerresource',
        templateUrl: '/Resources/Offer',
        data: {
            label: 'Offer resources' //label to show in breadcrumbs
        }
    }

    var findresource = {
        name: 'findresource',
        url: '/findresource',
        templateUrl: '/Resources/Index',
        data: {
            label: 'Looking for resources' //label to show in breadcrumbs
        }
    }

    var resourcedetail = {
        name: 'resourcedetail',
        url: '/resourcedetail/:resourceId',
        templateUrl: '/Resources/Detail',
        data: {
            label: 'Resource details' //label to show in breadcrumbs
        }
    }

    var resourceRequests = {
        name: 'resourceRequests',
        url: '/resourcerequests/',
        templateUrl: '/Resources/ResourceRequests',
        data: {
            label: 'Resource requests' //label to show in breadcrumbs
        }
    }

    var messages = {
        name: 'messages',
        url: '/messages/:threadId',
        templateUrl: '/Messages/Index',
        data: {
            label: 'Conversations' //label to show in breadcrumbs
        },
        params: {
            threadId: { squash: true, value: null }
        }
    }


    var votesettings = {
        name: 'votesettings',
        url: '/votesettings',
        templateUrl: '/Votings/Settings',
        data: {
            label: 'Vote Settings' //label to show in breadcrumbs
        }
    }

    var onboarding = {
        name: 'onboarding',
        url: '/onboarding/:onboardingprojectId',
        templateUrl: '/Votings/Onboarding',
        data: {
            label: 'Onboarding' //label to show in breadcrumbs
        },
        params: {
            onboardingprojectId: { squash: true, value: null }
        }
    }

    var applynomination = {
        name: 'applynomination',
        url: '/applynomination',
        templateUrl: '/Votings/ApplyNomination',
        data: {
            label: 'Apply for Nomination' //label to show in breadcrumbs
        }
    }

    var nominations = {
        name: 'nominations',
        url: '/nominations',
        templateUrl: '/Votings/Nominations',
        data: {
            label: 'Nominations' //label to show in breadcrumbs
        }
    }

    var nominationdetail = {
        name: 'nominations.nominationdetail',
        url: '/nominationdetail/:nominationId',
        templateUrl: '/Votings/NominationDetail',
        data: {
            label: 'Nomination Detail' //label to show in breadcrumbs
        }
    }

    var votings = {
        name: 'votings',
        url: '/votings',
        templateUrl: '/Votings/Voting',
        data: {
            label: 'Votings' //label to show in breadcrumbs
        }
    }

    var votingdetail = {
        name: 'votings.votingdetail',
        url: '/votingdetail/:votingsId',
        templateUrl: '/Votings/VotingDetail',
        data: {
            label: 'Voting Detail' //label to show in breadcrumbs
        }
    }

    var votingresults = {
        name: 'votingresults',
        url: '/votingresults',
        templateUrl: '/Votings/VotingResult',
        data: {
            label: 'Voting Results' //label to show in breadcrumbs
        }
    } //

    var votingresultdetail = {
        name: 'votingresults.votingresultdetail',
        url: '/votingresultdetail/:votingResultId',
        templateUrl: '/Votings/VotingResultDetail',
        data: {
            label: 'Voting Result Detail' //label to show in breadcrumbs
        }
    }


    //Production module routes

    var home = {
        name: 'home',
        url: '/:id/dashboard',
        templateUrl: '/ProjectManagement/_dashboard',
        controller: 'ProjectDashboardCtrl',
        data: {
            label: 'Dashboard' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }
    //generic route for file profile....
    var mainFileProfile = {
        name: 'home.fileProfile',
        url: '/fileProfile/:fileId',
        templateUrl: '/ProjectManagement/fileProfile',
        controller: 'fileProfile-ctrl',
        params: {
            fileId: null,
            name: null
        },
        data: {
            label: 'File profile' //label to show in breadcrumbs
        },
        resolve: {
            fileId: function ($stateParams) {
                return $stateParams.fileId;
            }
        }
    }

    var documents = {
        name: 'documents',
        url: '/:id/documents',
        templateUrl: '/ProjectManagement/documents',
        controller: 'documents-ctrl',
        resolve: {
            pId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return { prop: $stateParams.id }
            }
        },
        data: {
            label: 'Documents and Files' //label to show in breadcrumbs
        }
    }

    var documentcategory = {
        name: 'documents.documentcategory',
        url: '/documentcategory/:docId',
        templateUrl: '/ProjectManagement/documentcategory',
        controller: 'documentcategory-ctrl',
        params: {
            docId: null,
            name: null
        },
        resolve: {
            pId: function ($stateParams) {
                return {
                    prop: pId
                }
            }
        },
        data: {
            label: 'Document category' //label to show in breadcrumbs
        }
    }

    var files = {
        name: 'documents.documentcategory.files',
        url: '/files/:docCatId',
        templateUrl: '/ProjectManagement/files',
        controller: 'files-ctrl',
        cache: false,
        reloadOnSearch: false,
        params: {
            docCatId: null,
            name: null
        },
        data: {
            label: 'Documents' //label to show in breadcrumbs
        },
        resolve: {
            id: function ($state, $stateParams) {
                return $stateParams.docCatId;
            }
        }
    }

    var filesProfile = {
        name: 'documents.documentcategory.files.fileProfile',
        url: '/fileProfile/:fileId',
        templateUrl: '/ProjectManagement/fileProfile',
        controller: 'fileProfile-ctrl',
        params: {
            fileId: null,
            name: null
        },
        data: {
            label: 'File profile' //label to show in breadcrumbs
        },
        resolve: {
            fileId: function ($stateParams) {
                return $stateParams.fileId;
            }
        }
    }

    var fileProfile = {
        name: 'fileProfile',
        url: '/fileProfile/:fileId',
        templateUrl: '/ProjectManagement/fileProfile',
        controller: 'fileProfile-ctrl',
        params: {
            fileId: null,
            name: null
        },
        data: {
            label: 'File profile' //label to show in breadcrumbs
        },
        resolve: {
            fileId: function ($stateParams) {
                return $stateParams.fileId;
            }
        }
    }

    var crew = {
        name: 'crew',
        url: '/:id/crew',
        templateUrl: '/ProjectManagement/crew',
        controller: 'CrewController',
        data: {
            label: 'Crew' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }
    var scenesandscripts = {
        name: 'scenesandscripts',
        url: '/:id/scenesandscripts',
        templateUrl: '/ProjectManagement/scenesandscripts',
        controller: 'ScenesController',
        data: {
            label: 'Scenes & Script' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }
    var scene_details = {
        name: 'scenesandscripts.scene_details',
        url: '/scene_details/:sceneId',
        templateUrl: '/ProjectManagement/scene_details',
        controller: 'scene_details_ctrl',
       
        data: {
            label: 'Scene Details' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }

    var sceneFileProfile = {
        name: 'scenesandscripts.scene_details.fileProfile',
        url: '/fileProfile/:fileId',
        templateUrl: '/ProjectManagement/fileProfile',
        controller: 'fileProfile-ctrl',
        params: {
            fileId: null,
            name: null
        },
        data: {
            label: 'File profile' //label to show in breadcrumbs
        },
        resolve: {
            fileId: function ($stateParams) {
                return $stateParams.fileId;
            }
        }
    }

    var cast = {
        name: 'cast',
        url: '/:id/cast',
        templateUrl: '/ProjectManagement/cast',
        controller: 'CastController',
        data: {
            label: 'Cast' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }
   
    var character_details = {
        name: 'cast.character_details',
        url: '/character_details/:charId',
        templateUrl: '/ProjectManagement/character_details',
        controller: 'character_details_ctrl',

        data: {
            label: 'Characters' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }
    var extra_details = {
        name: 'cast.extra_details',
        url: '/extra_details/:charId',
        templateUrl: '/ProjectManagement/extra_details',
        controller: 'extra_details_ctrl',

        data: {
            label: 'Characters' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }
    var actor_details = {
        name: 'cast.actor_details',
        url: '/actor_details/:charId',
        templateUrl: '/ProjectManagement/actor_details',
        controller: 'actor_details_ctrl',

        data: {
            label: 'Actors' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }
    var talent_details = {
        name: 'cast.talent_details',
        url: '/talent_details/:charId',
        templateUrl: '/ProjectManagement/talent_details',
        controller: 'talent_details_ctrl',

        data: {
            label: 'Talents' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }
    var agency_details = {
        name: 'cast.agency_details',
        url: '/agency_details/:charId',
        templateUrl: '/ProjectManagement/agency_details',
        controller: 'agency_details_ctrl',

        data: {
            label: 'Agencies' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }

    var crewUnit = {
        name: 'crew.unit',
        url: '/unit/:unitId/:title',
        templateUrl: '/ProjectManagement/unit',
        controller: 'UnitController',
        data: {
            label: 'Unit' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }

    var crewGroup = {
        name: 'crew.group',
        url: '/group/:groupId/',
        templateUrl: '/ProjectManagement/group',
        controller: 'GroupController',
        data: {
            label: 'Group' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }

    var crewUserDetail = {
        name: 'crew.userdetail',
        url: '/userdetail/:userId/:title',
        templateUrl: '/ProjectManagement/userDetail',
        controller: 'UserDetailController',
        data: {
            label: 'User detail' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    } //

    var crewExternalUserDetail = {
        name: 'crew.externaluserdetail',
        url: '/externaluserdetail/:userId/:title',
        templateUrl: '/ProjectManagement/externalUserDetail',
        controller: 'ExternalUserDetailController',
        data: {
            label: 'External User detail' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }

    var crewUnitUserDetail = {
        name: 'crew.unit.userdetail',
        url: '/userdetail/:userId/:title',
        templateUrl: '/ProjectManagement/userDetail',
        controller: 'UserDetailController',
        data: {
            label: 'User detail' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }

    var crewGroupUserDetail = {
        name: 'crew.group.userdetail',
        url: '/userdetail/:userId/:title',
        templateUrl: '/ProjectManagement/userDetail',
        controller: 'UserDetailController',
        data: {
            label: 'User detail' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }

    var crewUnitExternalUserDetail = {
        name: 'crew.unit.externaluserdetail',
        url: '/externaluserdetail/:userId/:title',
        templateUrl: '/ProjectManagement/externalUserDetail',
        controller: 'ExternalUserDetailController',
        data: {
            label: 'External User detail' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }

    var crewGroupExternalUserDetail = {
        name: 'crew.group.externaluserdetail',
        url: '/externaluserdetail/:userId/:title',
        templateUrl: '/ProjectManagement/externalUserDetail',
        controller: 'ExternalUserDetailController',
        data: {
            label: 'External User detail' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }

    //task screen with tabs

    var task = {
        name: 'tasks',
        url: '/:id/tasks',
        abstract: true,
        templateUrl: '/ProjectManagement/tasks',
        controller: function ($scope) {

            $scope.tabs = [
                {
                    heading: 'My tasks',
                    route: 'tasks.mytasks',
                    active: true
                },
                {
                    heading: 'My created tasks',
                    route: 'DocumentoMasterView.B',
                    active: false
                },
                {
                    heading: 'Completed tasks',
                    route: 'DocumentoMasterView.B',
                    active: false
                },
                {
                    heading: 'All tasks',
                    route: 'DocumentoMasterView.B',
                    active: false
                }
            ];
        },
        data: {
            label: 'Tasks' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }

    var mytasks = {
        name: 'tasks.mytasks',
        url: '/mytasks',
        templateUrl: '/ProjectManagement/mytasks',
        controller: 'mytasksController',
        data: {
            label: 'My tasks' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }

    var mycreatedtasks = {
        name: 'tasks.mycreatedtasks',
        url: '/mycreatedtasks',
        templateUrl: '/ProjectManagement/mycreatedtasks',
        controller: 'mycreatedtasksCtrl',
        data: {
            label: 'My created tasks' //label to show in breadcrumbsf
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }

    var completedtasks = {
        name: 'tasks.completedtasks',
        url: '/completedtasks',
        templateUrl: '/ProjectManagement/completedtasks',
        controller: 'completedtasksCtrl',
        data: {
            label: 'Completed tasks' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }

    var alltasks = {
        name: 'tasks.alltasks',
        url: '/alltasks',
        templateUrl: '/ProjectManagement/alltasks',
        controller: 'alltasksCtrl',
        data: {
            label: 'All tasks' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }


    var taskprofile = {
        name: 'tasks.mytasks.taskprofile',
        url: '/taskprofile/:taskId',
        templateUrl: '/ProjectManagement/taskprofile',
        controller: 'taskProfileController',
        data: {
            label: 'Task profile' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }

    var taskfileprofile = {
        name: 'tasks.mytasks.taskprofile.fileProfile',
        url: '/fileProfile/:fileId',
        templateUrl: '/ProjectManagement/fileProfile',
        controller: 'fileProfile-ctrl',
        params: {
            fileId: null,
            name: null
        },
        data: {
            label: 'File profile' //label to show in breadcrumbs
        },
        resolve: {
            fileId: function ($stateParams) {
                return $stateParams.fileId;
            }
        }
    }

    //end task screen with tabs  projects  



    //announcements screens

    var announcements = {
        name: 'announcements',
        url: '/:id/announcements',
        templateUrl: '/Announcements/Index',
        controller: 'AnnouncementsController',
        data: {
            label: 'Announcements' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }

    var announcementprofile = {
        name: 'announcements.announcementprofile',
        url: '/announcementprofile/:announcementId',
        templateUrl: '/Announcements/announcementprofile',
        controller: 'announcementProfileController',
        data: {
            label: 'Announcement profile' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }

    var announcementfileprofile = {
        name: 'announcements.announcementprofile.fileProfile',
        url: '/fileProfile/:fileId',
        templateUrl: '/ProjectManagement/fileProfile',
        controller: 'fileProfile-ctrl',
        params: {
            fileId: null,
            name: null
        },
        data: {
            label: 'File profile' //label to show in breadcrumbs
        },
        resolve: {
            fileId: function ($stateParams) {
                return $stateParams.fileId;
            }
        }
    }

    //end announcements screens  announcementfileprofile

    //calendar screens

    var calendar = {
        name: 'calendar',
        url: '/:id/calendar',
        templateUrl: '/Calendar/Index',
        controller: 'CalendarController',
        data: {
            label: 'Calendar' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }

    var myCalendar = { //Discovery my calendar screen
        name: 'myCalendar',
        url: '/myCalendar',
        templateUrl: '/Calendar/MyCalendar',
        controller: 'MyCalendarController',
        data: {
            label: 'My Calendar' //label to show in breadcrumbs
        }
    }

    var calendarprofile = {
        name: 'calendar.calendarprofile',
        url: '/calendarprofile/:calendarId',
        templateUrl: '/Calendar/CalendarProfile',
        controller: 'CalendarProfileController',
        data: {
            label: 'Calendar Profile' //label to show in breadcrumbs
        },
        resolve: {
            projectId: function ($stateParams, MenuService) {
                MenuService.set($stateParams.id);
                return $stateParams.id;
            }
        }
    }

    var calendarfileprofile = {
        name: 'calendar.calendarprofile.fileProfile',
        url: '/fileProfile/:fileId',
        templateUrl: '/ProjectManagement/fileProfile',
        controller: 'fileProfile-ctrl',
        params: {
            fileId: null,
            name: null
        },
        data: {
            label: 'File profile' //label to show in breadcrumbs
        },
        resolve: {
            fileId: function ($stateParams) {
                return $stateParams.fileId;
            }
        }
    }


    $stateProvider.state(calendarfileprofile);
    $stateProvider.state(calendarprofile);
    $stateProvider.state(announcementfileprofile);
    $stateProvider.state(announcementprofile);
    $stateProvider.state(announcements);
    $stateProvider.state(calendar);
    $stateProvider.state(myCalendar);
    $stateProvider.state(createnewproject);
    $stateProvider.state(messages);
    $stateProvider.state(offerresource);
    $stateProvider.state(findresource);
    $stateProvider.state(resourcedetail);
    $stateProvider.state(resourceRequests);
    $stateProvider.state(publicprojectdetail);
    $stateProvider.state(offerfunding);
    $stateProvider.state(lookingforfunding);
    $stateProvider.state(fundingRequests);
    $stateProvider.state(projectpartner);
    $stateProvider.state(projectpartnerrequests);
    $stateProvider.state(offers);
    $stateProvider.state(hirerequests);
    $stateProvider.state(professionaldetails);
    $stateProvider.state(profile);
    $stateProvider.state(professionals);
    $stateProvider.state(disputestome);
    $stateProvider.state(mydisputes);
    $stateProvider.state(myprojects);
    $stateProvider.state(publicprojects);
    $stateProvider.state(notificationsettings);
    $stateProvider.state(paymentgateway);
    $stateProvider.state(admindashboard);
    $stateProvider.state(overview);
    $stateProvider.state(votesettings);
    $stateProvider.state(onboarding);
    $stateProvider.state(applynomination);
    $stateProvider.state(nominations);
    $stateProvider.state(nominationdetail);
    $stateProvider.state(votings);
    $stateProvider.state(votingdetail);
    $stateProvider.state(votingresults);
    $stateProvider.state(votingresultdetail);


    //Production module routes
    $stateProvider.state(dashboard);
    $stateProvider.state(documents);
    $stateProvider.state(documentcategory);
    $stateProvider.state(files);
    $stateProvider.state(home);
    $stateProvider.state(mainFileProfile);
    $stateProvider.state(filesProfile);
    $stateProvider.state(fileProfile);
    $stateProvider.state(crew);//crew
    $stateProvider.state(crewUnit);//crew unit
    $stateProvider.state(crewGroup);//crew group
    $stateProvider.state(crewUserDetail);//crew user detail
    $stateProvider.state(crewUnitUserDetail);//crew unit user detail
    $stateProvider.state(crewGroupUserDetail);//crew group user detail 
    $stateProvider.state(crewExternalUserDetail);//crew external user detail
    $stateProvider.state(crewUnitExternalUserDetail);//crew unit external user detail
    $stateProvider.state(crewGroupExternalUserDetail);//crew unit external user detail

    $stateProvider.state(task);//crew group user detail mytasks
    $stateProvider.state(mytasks);//crew group user detail mytasks mycreatedtasks
    $stateProvider.state(mycreatedtasks);//crew group user detail mytasks mycreatedtasks taskprofile
    $stateProvider.state(taskprofile);//crew group user detail mytasks mycreatedtasks 
    $stateProvider.state(completedtasks);//crew group user detail mytasks mycreatedtasks 
    $stateProvider.state(alltasks);//crew group user detail mytasks mycreatedtasks 
    $stateProvider.state(taskfileprofile);
    $stateProvider.state(scenesandscripts);
    $stateProvider.state(sceneFileProfile);
    $stateProvider.state(scene_details);
    $stateProvider.state(cast);
    $stateProvider.state(character_details);
    $stateProvider.state(extra_details);
    $stateProvider.state(actor_details);
    $stateProvider.state(talent_details);
    $stateProvider.state(agency_details);
});