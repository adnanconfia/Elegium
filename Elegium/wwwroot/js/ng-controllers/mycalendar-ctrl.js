
myApp.controller('MyCalendarController', function ($compile, $filter, $scope, $http, $state, $window, $window, $uibModal, $timeout, toaster, $ngConfirm, $state, uiCalendarConfig) {


    $scope.projectId = '';

    $scope.filters = {
        categories: [],
        search: '',
    };

    var initializeEventObj = function () {
        $scope.event = {
            Participant: [],
            Description: "",
            Title: "",
            StartTime: "",
            EndTime: "",
            ProjectId: "",


        };
    }
    initializeEventObj();
    $scope.eventsTab = [];
    $scope.events = [$scope.eventsTab];

    $scope.createEventByButton = function () {
        initializeEventObj();
        $scope.createNewEvent();
    }

    $scope.createEventBySelect = function (start, end) {
        console.log(start, end);

        $scope.event = {
            Id: 0,
            Title: '',
            Description: '',
            StartDate: new Date($filter('date')(start._d, 'dd-MMM-yyyy')),
            EndDate: new Date($filter('date')(end._d, 'dd-MMM-yyyy')),
            ProjectId: "",
        };

        $scope.event.EndDate.setDate($scope.event.EndDate.getDate() - 1);

        $scope.createNewEvent();
    }

    $scope.createNewEvent = function () {
        $('.popover').popover('hide');
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/calendar/create-event-template.html',
            controller: 'CreateMyEventCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                title: function () {
                    return 'Create a new event';
                },
                event: function () {
                    return $scope.event;
                },
                showProjectSelect: function () {
                    return true;
                }
            }
        });
        modalInstance.result.then(function (data) {
        }, function (data) {
            if (data)
                $scope.events.push(data);
        });
    }

    $scope.gotoEventProfile = function (projectId, eventId) {
        $('.popover').popover('hide');
        $state.go('calendar.calendarprofile', { id: projectId, calendarId: eventId });
    }

    $scope.gotoTaskProfile = function (projectId, eventId) {
        $('.popover').popover('hide');
        $state.go('tasks.mytasks.taskprofile', { id: projectId, taskId: eventId });
    }

    $scope.deleteEvent = function (eventId, calender) {
        $ngConfirm({
            title: 'Delete Event?',
            content: 'Are you sure to delete this event?',
            autoClose: 'cancel|10000',
            buttons: {
                deleteProject: {
                    text: 'Delete',
                    btnClass: 'btn-red',
                    action: function () {
                        $scope.deleteEventConfirmed(eventId, calender);
                    }
                },
                cancel: function () {

                }
            }
        });
    }

    $scope.deleteEventConfirmed = function (eventId, calender) {

        var event = null;
        var keepGoing = true;
        var index = 0;
        angular.forEach($scope.eventsTab, function (item, key) {
            console.log(item);
            if (keepGoing) {
                if (item.id == eventId) {
                    event = item;
                    keepGoing = false;
                    index = key;
                }
            }
        });

        if (event) {
            $http.delete(root + 'api/Calendar/DeleteEvent/' + eventId).then(resp => {
                if (resp.status == 200) {
                    $('.popover').popover('hide');
                    //event.remove();
                    uiCalendarConfig.calendars[calender].fullCalendar('removeEvents', eventId);
                }

            }, err => {
                toaster.pop({
                    type: 'error',
                    body: err,
                });
            });
        }
    }

    $scope.editEvent = function (eventId) {
        var event = null;
        var keepGoing = true;
        angular.forEach($scope.eventsTab, function (item) {
            console.log(item);
            if (keepGoing) {
                if (item.id == eventId) {
                    event = item;
                    keepGoing = false;
                }
            }
        });

        if (event) {
            $('.popover').popover('hide');
            $scope.event = {
                Id: event.id != undefined ? event.id : 0,
                Title: event.title != undefined ? event.title : '',
                StartDate: new Date(event.start),
                EndDate: new Date(event.end),
                Description: event.description,
                StartTime: event.startTime,
                EndTime: event.endTime,
                Location: event.location,
                Color: event.backgroundColor,
                AssignedTo: event.AssignedTo,
                AdditionalViewers: event.AdditionalViewers,
                CalenderCategoryId: event.CalenderCategoryId,
                ProjectId: event.ProjectId
            };

            modalInstance = $uibModal.open({
                animation: false,
                templateUrl: root + 'js/ng-templates/calendar/create-event-template.html',
                controller: 'CreateMyEventCtrl',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    title: function () {
                        return 'Edit event';
                    },
                    event: function () {
                        return $scope.event;
                    },
                    showProjectSelect: function () {
                        return false;
                    }
                }
            });
            modalInstance.result.then(function (data) {
            }, function (data) {
                if (data)
                    $scope.events.push(data);
            });
        }
        else {
            toaster.pop({
                type: 'error',
                body: 'Error while editing event.',
            });
        }
    }

    $scope.duplicateEvent = function (eventId) {

        var event = null;
        var keepGoing = true;
        angular.forEach($scope.eventsTab, function (item) {
            console.log(item);
            if (keepGoing) {
                if (item.id == eventId) {
                    event = angular.copy(item);
                    keepGoing = false;
                }
            }
        });

        if (event) {
            $('.popover').popover('hide');
            $scope.event = {
                Id: 0,
                Title: event.title != undefined ? event.title : '',
                StartDate: new Date(event.start),
                EndDate: new Date(event.end),
                Description: event.description,
                StartTime: event.startTime,
                EndTime: event.endTime,
                Location: event.location,
                Color: event.backgroundColor,
                AssignedTo: event.AssignedTo,
                AdditionalViewers: event.AdditionalViewers,
                CalenderCategoryId: event.CalenderCategoryId,
                ProjectId: event.ProjectId
            };

            modalInstance = $uibModal.open({
                animation: false,
                templateUrl: root + 'js/ng-templates/calendar/create-event-template.html',
                controller: 'CreateMyEventCtrl',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    title: function () {
                        return 'Create new event';
                    },
                    event: function () {
                        return $scope.event;
                    },
                    showProjectSelect: function () {
                        return false;
                    }

                }
            });
            modalInstance.result.then(function (data) {
            }, function (data) {
                if (data)
                    $scope.events.push(data);
            });
        }
        else {
            toaster.pop({
                type: 'error',
                body: 'Error while duplicating event.',
            });
        }
    }

    $scope.searchTyped = function () {
        setTimeout(function () {
            $scope.eventSearch('Calendar');
        }, 1000);
    }

    $scope.eventSearch = function (calender) {

        $scope.filteredEvents = [];
        if ($scope.filters.search.length != 0) {
            $scope.filteredEvents = $scope.eventsTab.filter(function (item) {
                return ((item.title.toLowerCase().indexOf($scope.filters.search.toLowerCase()) >= 0));
            });


        }
        else {
            $scope.filteredEvents = $scope.eventsTab;
        }

        if ($scope.filters.categories.length > 0) {

            $scope.filteredEvents_untouched = angular.copy($scope.filteredEvents);

            $scope.filteredEvents = $scope.filteredEvents.filter(function (item) {
                return ($scope.filters.categories.indexOf(item.CalenderCategoryId) >= 0);
            });

            //without category filter
            if ($scope.filters.categories.indexOf(0) >= 0) {
                $.merge($scope.filteredEvents, $scope.filteredEvents_untouched.filter(function (item) {
                    return (item.CalenderCategoryId == undefined || item.CalenderCategoryId == null);
                }));
            }

            //tasks filter
            if ($scope.filters.categories.indexOf(-1) >= 0) {
                $.merge($scope.filteredEvents, $scope.filteredEvents_untouched.filter(function (item) {
                    return (item.isTask);
                }));
            }
        }

        uiCalendarConfig.calendars[calender].fullCalendar('removeEvents');
        uiCalendarConfig.calendars[calender].fullCalendar('addEventSource', $scope.filteredEvents);

        //console.log('$scope.filters.search:', $scope.filters.search);
    };

    $scope.showFilters = function () {
        $('#collapseExample').collapse("toggle")
    }

    $scope.selectCategory = function (catId) {
        if ($scope.filters.categories.indexOf(catId) >= 0) {
            $scope.filters.categories.splice($scope.filters.categories.indexOf(catId), 1);
        }
        else {
            $scope.filters.categories.push(catId);
        }

        $scope.eventSearch('Calendar');
    }

    $scope.exportICS = function () {
        var cal = ics();
        angular.forEach($scope.events[0], function (item, i) {
            if (item.allDay)
                cal.addEvent(item.title, item.description, item.location, new Date($filter('date')(item.start, 'dd-MMM-yyyy')), new Date($filter('date')(item.end, 'dd-MMM-yyyy')));
            else
                cal.addEvent(item.title, item.description, item.location, item.start, item.end);
        });
        //cal.addEvent(subject, description, location, begin, end);
        //cal.addEvent(subject, description, location, begin, end); // yes, you can have multiple events :-)
        cal.download('Calender_' + $filter('date')(new Date(), 'ddMMMyyyy_HHmmss'));
    }

    $scope.changeView = function (view, calender) {
        uiCalendarConfig.calendars[calender].fullCalendar('changeView', view);
    };

    $scope.getCalenderCategories = function () {
        $http.get(root + 'api/CalenderCategories/GetCalenderAllCategories').then(resp => {
            //console.log(resp);
            $scope.categories = resp.data;

        }, err => {
            toaster.pop({
                type: 'error',
                body: err,
            });
        });
    }

    $scope.getCalenderCategories();

    //Clear calendar  
    function clearCalendar() {
        if (uiCalendarConfig.calendars.Calendar != null) {
            uiCalendarConfig.calendars.Calendar.fullCalendar('removeEvents');
        }
    }
    //Gets all events from db
    $scope.fetchEventAndRenderCalendar = function () {

        clearCalendar();
        $http.get(root + 'api/Calendar/GetMyCalenderEvents').then(function success(response) {
            console.log('events:', response.data);
            angular.forEach(response.data, function (v, key) {
                //console.log(v.StartDate);
                var obj = {
                    id: v.Id,
                    title: v.Title,
                    description: v.Description,
                    start: new Date($filter('date')(v.StartDate, 'yyyy-MM-dd HH:mm') + " UTC"),
                    end: new Date($filter('date')(v.EndDate, 'yyyy-MM-dd HH:mm') + " UTC"),
                    backgroundColor: v.Color,
                    location: v.Location,
                    borderColor: v.Color,
                    startTime: v.StartTime,
                    endTime: v.EndTime,
                    AssignedTo: v.AssignedTo,
                    AdditionalViewers: v.AdditionalViewers,
                    CalenderCategoryId: v.CalenderCategoryId,
                    ProjectId: v.ProjectId,
                    ProjectName: v.Project.Name,
                    allDay: v.AllDay
                }

                $scope.eventsTab.push(obj);

            });

            $scope.getAllPendingTasks();


        }, function error() {
        });
    }

    $scope.getAllPendingTasks = function () {
        //GetAllPendingTasks
        $http.get(root + 'api/Tasks/GetAllProjectsPendingTasks').then(function success(response) {
            console.log('tasks:', response.data);
            angular.forEach(response.data, function (v, key) {
                //console.log(v.StartDate);
                var obj = {
                    id: v.Id,
                    title: 'Deadline: ' + v.Title,
                    description: v.Description,
                    start: new Date(v.Deadline),
                    end: new Date(v.Deadline),
                    backgroundColor: '#ffcd01',
                    //location: v.Location,
                    borderColor: '#ffcd01',
                    isTask: true,
                    //startTime: v.StartTime,
                    //endTime: v.EndTime,
                    AssignedTo: v.AssignedTo,
                    editable: false,
                    ProjectId: v.ProjectId,
                    ProjectName: v.Project.Name,
                    //AdditionalViewers: v.AdditionalViewers,
                    //CalenderCategoryId: v.CalenderCategoryId,
                    allDay: true
                }

                $scope.eventsTab.push(obj);
                angular.forEach(v.SubTasks, function (subTask, i) {
                    var obj2 = {
                        id: subTask.Id,
                        title: 'Deadline: ' + subTask.Title,
                        description: subTask.Description,
                        start: new Date(subTask.Deadline),
                        end: new Date(subTask.Deadline),
                        backgroundColor: '#ffcd01',
                        borderColor: '#ffcd01',
                        isTask: true,
                        AssignedTo: subTask.AssignedTo,
                        editable: false,
                        ProjectId: v.ProjectId,
                        ProjectName: v.Project.Name,
                        allDay: true
                    }
                    $scope.eventsTab.push(obj2);
                });

            });
            console.log('$scope.eventsTab', $scope.eventsTab);
        }, function error() {
        });
    }

    $scope.fetchEventAndRenderCalendar();

    //Configure Calendar -mubi calendar  
    $scope.uiConfig = {
        calendar: {
            height: 'auto',
            editable: true,
            timeFormat: 'H:mm',
            forceEventDuration: true,
            displayEventTime: true,
            header: {
                left: 'title',
                center: '',
                right: 'prev,next today'
            },
            eventDrop: function (event) {
                $scope.eventResized(event);
            },
            eventResize: function (event) {
                $scope.eventResized(event);
                //console.log(event);
            },
            selectable: true,
            select: function (start, end) {
                $scope.createEventBySelect(start, end);
            },
            eventRender: function (event, element) {
                var s = $filter('date')(event.start._d, 'dd-MMM-yyyy');
                var e = $filter('date')(event.end._d, 'dd-MMM-yyyy');

                var dateString = s;

                if (s == e) {
                    if (!event.startTime && !event.endTime) {
                        dateString += ' all day';
                    }
                    else {
                        if (event.startTime)
                            dateString += ' ' + event.startTime;
                        if (event.endTime)
                            dateString += ' till ' + event.endTime;
                    }
                }
                else {
                    if (event.startTime)
                        dateString += ' ' + event.startTime;
                    dateString += ' till ' + e;
                    if (event.endTime)
                        dateString += ' ' + event.endTime;
                }

                //console.log(event);

                var assginedToHtml = "";
                angular.forEach(event.AssignedTo, function (item) {
                    if (item.Type == 'user') {
                        assginedToHtml += `<img style="width:25px; height:25px"
                                            data-toggle="tooltip" data-placement="top" tooltip title="`+ item.name + `"
                                            src="/api/UserProfiles/GetUserPhoto/`+ item.id + `/15/15'" alt="Avatar">`;
                    }
                    else {
                        assginedToHtml += `<img style="width:25px; height:25px"
                                            data-toggle="tooltip" data-placement="top" tooltip title="`+ item.name + `"
                                            src="`+ item.icon + `" alt="Avatar">`;
                    }
                });

                var userAssignedHtml = '';
                if (assginedToHtml != "") {
                    userAssignedHtml = `<div class="mb-3" style="display:flex">
                                            <i class="fa fa-users mt-3"></i>
                                            <ul class="list-unstyled team-info sm m-0 ml-3 overflow-hidden text-nowrap" style="height:35px">
                                                <li class="mt-2">
                                                    `+ assginedToHtml + `
                                                </li>
                                            </ul>
                                        </div>`
                }

                var locationHtml = '';
                if (event.location) {
                    locationHtml = `<p><i class="fa fa-map-marker"></i> <span>` + event.location + `</span></p>`;
                }

                var finalHtml = '';
                if (!event.isTask) {
                    finalHtml = `<div class="row">
                                    <div class="col-12">
                                        <h4>`+ event.title + `</h4>
                                        <p>`+ event.description + `</p>
                                        <p><i class="fa fa-clock-o"></i> <span>`+ dateString + `</span></p>
                                        `+ locationHtml + `
                                        <p><i class="fa fa-briefcase"></i> <span>`+ event.ProjectName + `</span></p>
                                        `+ userAssignedHtml + `
                                    </div>
                                    <div class="col-12 text-right">
                                        <a href="javascript:void(0)" class="btn btn-secondary text-white cursor-pointer" title="Edit" ng-click="editEvent(`+ event.id + `)"><i class="fa fa-edit"></i></a>
                                        <a href="javascript:void(0)" class="btn btn-secondary text-white cursor-pointer" title="Duplicate event" ng-click="duplicateEvent(`+ event.id + `)"><i class="fa fa-files-o"></i></a>
                                        <a href="javascript:void(0)" class="btn btn-danger text-white cursor-pointer" title="Delete" ng-click="deleteEvent(`+ event.id + `,'Calendar')"><i class="fa fa-trash"></i></a>
                                        <a href="javascript:void(0)" class="btn btn-secondary text-white cursor-pointer" title="Detail" ng-click="gotoEventProfile(`+ event.ProjectId + `,` + event.id + `)"><i class="fa fa-ellipsis-h"></i></a>
                                    </div>
                                </div>`;
                }
                else {
                    var descriptionString = event.description == null ? '' : event.description;
                    finalHtml = `<div class="row">
                                    <div class="col-12">
                                        <h4>`+ event.title + `</h4>
                                        <p>`+ descriptionString + `</p>
                                        <p><i class="fa fa-clock-o"></i> <span>`+ s + `</span></p>
                                        <p><i class="fa fa-briefcase"></i> <span>`+ event.ProjectName + `</span></p>
                                        `+ locationHtml + `
                                        `+ userAssignedHtml + `
                                    </div>
                                    <div class="col-12 text-right">
                                        <a href="javascript:void(0)" class="btn btn-secondary text-white cursor-pointer" title="Detail" ng-click="gotoTaskProfile(`+ event.ProjectId + `,` + event.id + `)"><i class="fa fa-ellipsis-h"></i></a>
                                    </div>
                                </div>`;
                }

                //if (event.id == 25) {
                //    console.log('mubi', userAssignedHtml);
                //}

                element.popover({
                    animation: true,
                    html: true,
                    autoClose: true,
                    content: $compile(finalHtml)($scope),
                    trigger: 'click'
                });

                element.find('.fc-title').append('<div class="hr-line-solid-no-margin"></div><span class="font-10 text-white-50">' + event.ProjectName + '</span></div>');
            },
        }

    };

    $scope.eventDropped = function (event) {
        console.log(event);
        if (event.allDay) {
            myEvent = {
                Id: event.id,
                StartDate: event.start._d,
                EndDate: event.end._d
            }
        }
        else {
            var s = event.start._i;
            var e = event.end._i;
            var start = new Date(s[0] + '-' + (Number(s[1]) + 1) + '-' + s[2] + ' ' + s[3] + ':' + s[4]);
            var end = new Date(e[0] + '-' + (Number(e[1]) + 1) + '-' + e[2] + ' ' + e[3] + ':' + e[4]);
            myEvent = {
                Id: event.id,
                StartDate: start,
                EndDate: end,
                StartTime: $filter('date')(start, 'hh:mm a'),
                EndTime: $filter('date')(end, 'hh:mm a')
            }
        }

        //console.log('myEvent',myEvent);

        //$http.post(root + 'api/Calendar/PostEvent', myEvent).then(resp => {
        //    if (resp.status == 200) {

        //    }

        //}, err => {
        //});
    }
    $scope.eventResized = function (event) {

        if (event.isTask) return false;

        var myEvent = {};
        if (event.allDay) {
            myEvent = {
                Id: event.id,
                StartDate: event.start._d,
                EndDate: event.end._d
            }
        }
        else {
            var s = event.start._i;
            var e = event.end._i;
            var start = new Date(s[0] + '-' + (Number(s[1]) + 1) + '-' + s[2] + ' ' + s[3] + ':' + s[4]);
            var end = new Date(e[0] + '-' + (Number(e[1]) + 1) + '-' + e[2] + ' ' + e[3] + ':' + e[4]);
            myEvent = {
                Id: event.id,
                StartDate: start,
                EndDate: end,
                StartTime: $filter('date')(start, 'hh:mm a'),
                EndTime: $filter('date')(end, 'hh:mm a')
            }
        }

        console.log(myEvent);

        $http.post(root + 'api/Calendar/PostEvent', myEvent).then(resp => {
            if (resp.status == 200) {

            }

        }, err => {
        });
    }


    $scope.openCalenderCategories = function () {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/calendar/calender-categories-template.html',
            controller: 'CalenderCategoriesCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                title: function () {
                    return 'Calendar Categories';
                },
                projectId: function () {
                    return $scope.projectId;
                }
            }
        });

        modalInstance.result.then(function (data) {

        }, function (data) {
            if (data) {
                if (!isNaN(data)) $scope.filters.categories = [data];
                $scope.eventSearch('Calendar');
            }

        });
    }

}).controller('CreateMyEventCtrl', function ($scope, $state, $rootScope, $uibModal, $uibModalInstance, $stateParams, $http, $timeout, toaster, title, event, showProjectSelect) {

    $uibModalInstance.rendered.then(function () {


        $scope.title = title;
        $scope.event = event;
        $scope.showProjectSelect = showProjectSelect;

        if ($scope.event.StartTime || $scope.event.EndTime) {
            $scope.showTime = true;
        }

        $scope.options = {
            inputClass: 'border-0',
            format: 'hexString'
        };


        $scope.$watch('event.StartTime', function () {
            $scope.event.EndTime = angular.copy($scope.event.StartTime);
        });

        $scope.getProjects = function () {
            $http.get(root + 'api/Projects/GetProjects').then(function success(response) {
                $scope.projects = response.data;
                console.log($scope.projects);
            }, function error() { });
        }

        $scope.getCalenderCategories = function () {
            $http.get(root + 'api/CalenderCategories/GetCalenderCategories/' + $scope.event.ProjectId).then(resp => {
                //console.log(resp);
                $scope.categories = resp.data;

            }, err => {

            });
        }

        $scope.getEventDocumentFiles = function () {
            $http.get(root + 'api/DocumentFiles/GetEventDocumentFiles/' + $scope.event.Id).then(resp => {
                //console.log(resp);
                $scope.files = resp.data;

            }, err => {

            });
        }

        if ($scope.event.Id != 0) {
            $scope.getEventDocumentFiles();
        }

        $scope.projectChanged = function () {
            if ($scope.event.ProjectId) {
                $scope.getProjectUsersAndGroups();
                $scope.getCalenderCategories();
            }
        }


        $scope.getProjects();
        //$scope.getCalenderCategories();

        $scope.getProjectUsersAndGroups = function () {
            $http.get(root + 'api/Comments/GetProjectUsersAndGroups/' + $scope.event.ProjectId).then(resp => {
                $scope.projectUsersAndGroups = resp.data;
            }, err => {
            });
        }

        if ($scope.event.ProjectId) {
            $scope.getProjectUsersAndGroups();
            $scope.getCalenderCategories();
        }

        $scope.groupUsers = function (item) {
            return item.type == 'user' ? 'Users' : (item.type == 'units' ? 'Units' : (item.type == 'groups' ? 'Groups' : 'Users'));
        }

        $scope.createEvent = function () {

            //console.log($scope.event.StartDate);
            //console.log($scope.event);

            if (!$scope.event.StartDate || !$scope.event.EndDate || !$scope.event.Title || !$scope.event.Description) {
                toaster.pop({
                    type: 'error',
                    body: 'Please fill the required fields.',
                });
                return false;
            }

            $http.post(root + 'api/Calendar/PostEvent/', $scope.event).then(resp => {
                if (resp.status == 200) {
                    var eventId = resp.data.Id;
                    $scope.uppy.upload().then((result) => {
                        var files = Array.from(result.successful);
                        $scope.uploadedFiles = [];
                        files.forEach((file) => {
                            // file: { id, name, type, ... }
                            // progress: { uploader, bytesUploaded, bytesTotal }
                            var resp = file.response.uploadURL;
                            var id = resp.substring(resp.lastIndexOf("/") + 1, resp.length);
                            var fileObj = {};
                            fileObj.FileId = id;
                            fileObj.Name = file.name;
                            fileObj.Size = file.size;
                            fileObj.ContentType = file.type;
                            var validImageTypes = ['image/gif', 'image/jpeg', 'image/png'];
                            if (validImageTypes.includes(file.type)) {
                                // invalid file type code goes here.
                                fileObj.Type = "image";
                            }
                            fileObj.EventId = eventId;
                            fileObj.Default = $scope.isItDocDacThumbnail;
                            $scope.uploadedFiles.push(fileObj);
                            $scope.files.push(fileObj);
                        });

                        if ($scope.uploadedFiles.length > 0) {
                            $http.post(root + 'api/DocumentFiles/PostDocumentFiles', $scope.uploadedFiles).then(
                                function success(resp) {
                                    if (resp.data.length > 0) {

                                        $scope.uppy.reset();
                                    }
                                    if ($scope.event.Id) {
                                        toaster.pop({
                                            type: 'success',
                                            body: 'Event updated successfully.',
                                        });
                                    }
                                    else {
                                        toaster.pop({
                                            type: 'success',
                                            body: 'Event created successfully.',
                                        });
                                    }

                                    $scope.cancel();
                                    $state.reload();
                                }
                                , function error() { });
                        }
                        else {
                            if ($scope.event.Id != 0) {
                                toaster.pop({
                                    type: 'success',
                                    body: 'Event updated successfully.',
                                });
                            }
                            else {
                                toaster.pop({
                                    type: 'success',
                                    body: 'Event created successfully.',
                                });
                            }
                            $scope.cancel();
                            $state.reload();
                        }
                    })

                }

            }, err => {
            });
        }

        $scope.selectCategory = function (id) {
            $scope.event.CalenderCategoryId = id;
            console.log($scope.event);
        }


        //files and images tab
        var holder;
        var holderchk = false;

        $scope.tabContentLength = {

        };

        holder = document.getElementById('draggableContainer');
        if (holder != null) {
            holderchk = true;
        } else {
            holderchk = false;
        }
        console.log("hello", holder);

        $scope.files = [];
        $scope.openProjectFileDialog = function () {
            $('#projectFile').click();
        }
        try {
            $scope.uppy = new Uppy.Core({ autoProceed: false })
                .use(Uppy.Dashboard, {
                    id: 'ringring',
                    target: '#uppy-uploader',
                    allowMultipleUploads: true,
                    //trigger: '#uppy-uploader',
                    metaFields: [],
                    //trigger: '#uppy-select-files',
                    inline: true,
                    height: 300,
                    //defaultTabIcon: defaultTabIcon,
                    showLinkToFileUploadResult: false,
                    showProgressDetails: false,
                    hideUploadButton: true,
                    hideRetryButton: false,
                    hidePauseResumeButton: false,
                    hideCancelButton: false,
                    hideProgressAfterFinish: false,
                    note: null,
                    closeModalOnClickOutside: false,
                    closeAfterFinish: false,
                    disableStatusBar: false,
                    disableInformer: false,
                    disableThumbnailGenerator: false,
                    disablePageScrollWhenModalOpen: true,
                    animateOpenClose: true,
                    fileManagerSelectionType: 'files',
                    proudlyDisplayPoweredByUppy: false,
                    //onRequestCloseModal: () => $scope.closeUppyModal(),
                    showSelectedFiles: true,
                    showRemoveButtonAfterComplete: false,
                    //locale: defaultLocale,
                    browserBackButtonClose: false,
                    theme: $rootScope.BackgroundThings.DarkMode ? 'dark' : 'light',
                })
                .use(Uppy.Tus,
                    {
                        endpoint: root + 'files/',
                        resume: true,
                        retryDelays: [0, 1000, 3000, 5000],
                        chunkSize: 5242880
                    });

            $scope.uppy.on('complete', (result) => {
                //var files = Array.from(result.successful);
                //$scope.uploadedFiles = [];
                //files.forEach((file) => {
                //    // file: { id, name, type, ... }
                //    // progress: { uploader, bytesUploaded, bytesTotal }
                //    var resp = file.response.uploadURL;
                //    var id = resp.substring(resp.lastIndexOf("/") + 1, resp.length);
                //    var fileObj = {};
                //    fileObj.FileId = id;
                //    fileObj.Name = file.name;
                //    fileObj.Size = file.size;
                //    fileObj.ContentType = file.type;
                //    var validImageTypes = ['image/gif', 'image/jpeg', 'image/png'];
                //    if (validImageTypes.includes(file.type)) {
                //        // invalid file type code goes here.
                //        fileObj.Type = "image";
                //    }
                //    fileObj.Default = $scope.isItDocDacThumbnail;
                //    $scope.uploadedFiles.push(fileObj);
                //    $scope.files.push(fileObj);
                //});
                //if ($scope.uploadedFiles.length > 0) {
                //    $scope.$apply(function () {
                //        $scope.hasFilesinUppy = true;
                //    });
                //    $scope.uppy.reset();
                //}
            });
            $scope.uppy.on('file-removed', (file, reason) => {
                $timeout(() => {
                    $scope.$apply(() => {
                        $scope.hasFilesinUppy = $scope.uppy.getFiles().length > 0
                    });
                });
            });
        }
        catch (err) {
            console.log('err', err);
        }
        $scope.hasFilesinUppy = false;

        $('#projectFile').on('change', function (event) {
            $scope.isItDocDacThumbnail = false;
            addFilesToUppy(event.target.files);
        });


        var lastTarget;

        if (holderchk) {
            holder.ondragenter = function (e) {
                lastTarget = e.target;
                e.preventDefault();
                e.preventDefault();
                this.className = ' project-hover-files';
                return false;
            };

            holder.ondragstart = function (e) {
                e.preventDefault();
                e.stopPropagation();
                return false;
            }

            holder.ondragover = function (e) {
                e.preventDefault();
                e.preventDefault();
                this.className = ' project-hover-files';
            }

            holder.ondragleave = function (e) {
                e.preventDefault();
                e.preventDefault();
                if (lastTarget === e.target) {
                    this.className = '';
                }
            };

            holder.ondragend = function (e) {
                e.preventDefault();
                e.stopPropagation();
                this.className = '';
            }

            holder.ondrop = function (e) {
                e.preventDefault();
                e.stopPropagation();
                this.className = 'card bg-none b-none';
                $scope.isItDocDacThumbnail = false;
                addFilesToUppy(e.dataTransfer.files);
            };
        }

        var addFilesToUppy = function (files) {
            //$scope.$apply(function () {
            $scope.hasFilesinUppy = true;
            //});

            Array.from(files).forEach((a) => {
                try {
                    $scope.uppy.addFile({
                        data: a,
                        name: a.name,
                        type: a.type
                    });
                }
                catch (erro) {
                    console.log(erro, 'bilal');
                }
            });
        }

        $scope.filesPaging = {
            page: 1,
            size: 12
        }

        $scope.deleteFile = function ($event, file) {
            $event.stopPropagation();
            var index = $scope.files.indexOf(file);
            if (index > -1)
                $scope.files.splice(index, 1);

            $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount - 1;
            if ($scope.tabContentLength.filesCount < 0) {
                $scope.tabContentLength.filesCount = 0;
            }

            $http.post(root + 'api/DocumentFiles/DeleteDocumentFiles/' + file.Id).then(resp => {
                //if (resp.
            }, err => {
                toaster.pop({
                    type: 'error',
                    body: err,
                });
            });

        }



        $scope.cancel = function () {
            $uibModalInstance.dismiss();
        }
    });
});
