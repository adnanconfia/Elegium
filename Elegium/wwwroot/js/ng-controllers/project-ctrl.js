myApp.controller('ProjectController', function ($scope, $filter, $http, $uibModal, toaster, $ngConfirm, $stateParams, ProjectService) {

    var modalInstance = null;



    //data fetch functions here below

    $scope.getProjects = function () {
        $http.get(root + 'api/Projects/GetProjects').then(function success(response) {
            $scope.projectsList = response.data;
            console.log($scope.projectsList);
        }, function error() { });
    }

    $scope.getProjects();


    //Logical functions here below

    $scope.showProjectOwnerLooking = function (project) {
        //console.log('project:',project);
        if (project.ProjectPartners.length > 0 ||
            project.ProjectPartnerRequirement.NeedFinancialParticipation ||
            (project.ProjectFunding && project.ProjectFunding.Amount)) {
            return true;
        }
        else
            return false;
    }


    $scope.openCreateProject = function () {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/project/create-project-template.html',
            controller: 'CreateProjectCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                title: function () {
                    return 'Create a new project';
                },
                projectItem: function () {
                    return null;
                }
            }
        });
        modalInstance.result.then(function () {

        }, function (data) {
            $scope.getProjects();
        });
    }

    //Navigatate to some specific tab by checking url
    if ($stateParams.newProject == 'Y') {
        $scope.openCreateProject();
    }

    $scope.editProject = function (project) {
        //console.log(project);
        $http.get(root + 'api/Projects/GetProject/' + project.Project.Id).then(function success(response) {
            modalInstance = $uibModal.open({
                animation: false,
                templateUrl: root + 'js/ng-templates/project/create-project-template.html',
                controller: 'CreateProjectCtrl',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    title: function () {
                        return 'Editing project';
                    },
                    projectItem: function () {
                        return response.data;;
                    }
                }
            });
            modalInstance.result.then(function () {
            }, function (data) {
                $scope.getProjects();
            });
        }, function error() { });


    }

    $scope.startProject = function (project) {

        $ngConfirm({
            title: 'Start Project?',
            content: 'Are you sure to start this Project?',
            autoClose: 'cancel|8000',
            buttons: {
                deleteProject: {
                    text: 'Start Project',
                    btnClass: 'btn-primary',
                    action: function () {
                        $http.get(root + 'api/Projects/StartProject/' + project.Id).then(function success(response) {
                            if (response.status == 200) {
                                toaster.pop({
                                    type: 'success',
                                    title: 'Success',
                                    body: 'Project started!',
                                });
                                $scope.getProjects();
                            }
                        }, function error(err) {
                            toaster.pop({
                                type: 'error',
                                title: 'Error',
                                body: err.data,
                            });
                        });
                    }
                },
                cancel: function () {

                }
            }
        });



    }

    $scope.finishProject = function (project) {

        $ngConfirm({
            title: 'Stop Project?',
            content: 'Are you sure to finish this Project? All the crew and resources would be released from the project',
            autoClose: 'cancel|8000',
            buttons: {
                deleteProject: {
                    text: 'Finish Project',
                    btnClass: 'btn-danger',
                    action: function () {
                        $http.get(root + 'api/Projects/FinishProject/' + project.Id).then(function success(response) {
                            if (response.status == 200) {
                                toaster.pop({
                                    type: 'success',
                                    title: 'Success',
                                    body: 'Project finihsed! All the crew and resources are released from the project.',
                                });
                                $scope.getProjects();
                            }
                        }, function error(err) {
                            toaster.pop({
                                type: 'error',
                                title: 'Error',
                                body: err.data,
                            });
                        });
                    }
                },
                cancel: function () {

                }
            }
        });

    }

    $scope.applyVoteFunding = function (project) {
        if (project.IsVoteFundingApplied) {
            toaster.pop({
                type: 'info',
                title: '',
                body: 'Vote funding is already applied for this project.',
            });
            return false;
        }
        if (!project.OnBoardingCompleted) {
            toaster.pop({
                type: 'info',
                title: '',
                body: 'Please complete the onboarding for this project to apply vote funding.',
            });
            return false;
        }

        $ngConfirm({
            title: 'Apply for vote funding?',
            content: 'Are you sure to apply for vote funding?',
            autoClose: 'cancel|8000',
            buttons: {
                apply: {
                    text: 'Apply',
                    btnClass: 'btn-primary',
                    action: function () {
                        $http.post(root + 'api/VoteFunding/ApplyVoteFunding/', project.Id).then(function success(response) {
                            if (response.status == 200) {
                                toaster.pop({
                                    type: 'success',
                                    title: '',
                                    body: 'Applied successfully.',
                                });
                                //$scope.getProjects();
                            }
                        }, function error(err) {
                            toaster.pop({
                                type: 'error',
                                title: '',
                                body: err.data,
                            });
                        });
                    }
                },
                cancel: function () {

                }
            }
        });
    }

    $scope.deleteProject = function (projectId) {

        $ngConfirm({
            title: 'Delete Project?',
            content: 'Are you sure want to delete this Project? This action can not be revert back.',
            autoClose: 'cancel|80000',
            buttons: {
                deleteProject: {
                    text: 'Delete Project',
                    btnClass: 'btn-red',
                    action: function () {
                        ProjectService.DeleteProject(projectId).then((succ) => {
                            $scope.getProjects();
                        });
                        // $scope.deleteConfirmProject(projectId);
                    }
                },
                cancel: function () {

                }
            }
        });



    }

    $scope.deleteConfirmProject = function (projectId) {

        $http.delete(root + 'api/Projects/DeleteProject/' + projectId).then(function success(response) {
            if (response.status == 200) {
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: 'Project deleted successfully!',
                });
                $scope.getProjects();
            }
        }, function error(err) {
            toaster.pop({
                type: 'error',
                title: 'Error',
                body: err.data,
            });
        });
    }


    //test method
    $scope.showToast = function () {
        toaster.pop({
            type: 'warning',
            title: 'Title text',
            body: 'Body text',
        });
    }

});

myApp.controller('CreateProjectCtrl', function ($scope, $uibModal, $uibModalInstance, $http, toaster, title, projectItem, ProjectService) {

    $scope.title = title;
    $scope.opacity = [
        { Id: 0, Name: "0" },
        { Id: 0.1, Name: "0.1" },
        { Id: 0.2, Name: "0.2" },
        { Id: 0.3, Name: "0.3" },
        { Id: 0.4, Name: "0.4" },
        { Id: 0.5, Name: "0.5" },
        { Id: 0.6, Name: "0.6" },
        { Id: 0.7, Name: "0.7" },
        { Id: 0.8, Name: "0.8" },
        { Id: 0.9, Name: "0.9" },
        { Id: 1, Name: "1" }
    ];
    //$scope.opacity = [0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1];

    ProjectService.getVisibilityAreas().then(function (response) {
        $scope.visibilityAreas = response;
    });

    ProjectService.getProjectManagementPhases().then(function (response) {
        $scope.ProjectManagementPhaseList = response;
    });

    $scope.modes = [
        { Id: "1", Name: "23.98p" },
        { Id: "2", Name: "24p" },
        { Id: "3", Name: "25p" },
        { Id: "4", Name: "30p" },
        { Id: "5", Name: "50i" },
        { Id: "6", Name: "60i" },
        { Id: "7", Name: "HFR" }
    ];

    $scope.productionColorType = [{ Id: "1", Name: "B/W" }, { Id: "1", Name: "Color" }];

    $scope.project = {
        Color: '#FF0000'
    };
    $scope.partnerReq = {};
    $scope.partners = [];
    $scope.lists = {};

    if (projectItem != undefined && projectItem != null) {
        $scope.project = projectItem.Project;
        $scope.partnerReq = projectItem.ProjectPartnerRequirement;
        $scope.partners = projectItem.ProjectPartners;
        $scope.projectFunding = projectItem.ProjectFunding;
        $scope.financialParticipation = projectItem.ProjectFinancialParticipation;
        $scope.ProjectLogo = projectItem.Project.Logo ? "data:image/jpg;base64," + projectItem.Project.Logo : null;
        //Lists
        $scope.lists.projectVisibilityAreas = projectItem.ProjectVisibilityAreas;
        //$scope.lists.partnerReqManagementPhases = projectItem.ProjectPartnerRequirementManagementPhases;
        //$scope.lists.projectFundingManagementPhases = projectItem.ProjectFundingManagementPhases;
        //$scope.lists.financialParticipationManagementPhases = projectItem.ProjectFinancialParticipationManagementPhases;
    }



    $scope.options = {
        inputClass: 'border-0',
        format: 'hexString'
    };



    $scope.getProductionType = function () {
        $http.get(root + 'api/ProductionTypes').then(function success(response) {
            $scope.productionTypes = response.data;
            //console.log($scope.productionTypes);
        }, function error() { });
    }

    $scope.getLanguages = function () {
        $http.get(root + 'api/Languages').then(function success(response) {
            $scope.languages = response.data;
            //console.log($scope.languages);
        }, function error() { });
    }

    $scope.getCurrencies = function () {
        $http.get(root + 'api/Currencies').then(function success(response) {
            $scope.currencies = response.data;
            //console.log($scope.languages);
        }, function error() { });
    }

    $scope.getProductionType();
    $scope.getLanguages();
    $scope.getCurrencies();



    //Logical functions

    $scope.openCroppie = function () {
        var modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/croppie-playground.html',
            controller: 'CroppiePlayCtrlCustom',
            size: 'lg',
            resolve: {
                title: function () {
                    return "Upload project cover photo";
                },
                image: function () {
                    return $scope.ProjectLogo;
                },
                width: function () { return 750 },
                height: function () { return 300 }
            }
        });
        modalInstance.result.then(function () {
            //on ok button press 
        }, function (data) {
            console.log(data, 'bilal');
            $scope.ProjectLogo = data.image;
            $scope.ProjectOriginalLogo = data.source;

        });
    }

    $scope.addPartner = function () {
        if ($scope.partnerReq.ProjectPartnersCount > $scope.partners.length) {
            $scope.partners.push({
                ProjectPartnerRole: '',
                FinancialParticipationRequired: false,
                FinancialShare: ''
            });
        }
        else {
            toaster.pop({
                type: 'info',
                title: '',
                body: "Cannot add more than " + $scope.partners.length + " partners!",
            });
        }
    }

    $scope.save = function () {

        $scope.projectViewModel = {
            Project: $scope.project,
            ProjectPartnerRequirement: $scope.partnerReq,
            ProjectPartners: $scope.partners,
            ProjectFunding: $scope.projectFunding,
            ProjectFinancialParticipation: $scope.financialParticipation,
            ProjectLogo: $scope.ProjectLogo,
            ProjectOriginalLogo: $scope.ProjectOriginalLogo,
            //Lists
            ProjectVisibilityAreas: $scope.lists.projectVisibilityAreas,
            //ProjectPartnerRequirementManagementPhases: $scope.lists.partnerReqManagementPhases,
            //ProjectFundingManagementPhases: $scope.lists.projectFundingManagementPhases,
            //ProjectFinancialParticipationManagementPhases: $scope.lists.financialParticipationManagementPhases
        };

        $http.post(root + 'api/Projects/SaveOrUpdateProject', $scope.projectViewModel).then(function success(response) {
            if (response.status == 200) {
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: 'Project saved successfully!',
                });
                $scope.cancel();
            }
        }, function error(err) {
            toaster.pop({
                type: 'error',
                title: 'Error',
                body: err.data,
            });
        });

        //console.log($scope.projectViewModel);

    }


    $scope.cancel = function () {
        $uibModalInstance.dismiss();
    }


    //bilal

    ProjectService.getProjectFundersRequired().then(function (response) {
        $scope.fundersRequired = response;
    });

    //end bilal
}).controller('CroppiePlayCtrlCustom', ['$scope', '$uibModalInstance', 'title', 'image', 'width', 'height', function ($scope, $uibModalInstance, title, image, width, height) {


    $uibModalInstance.opened.then(function () {


        $scope.title = title;
        //$scope.viewportWidth = 0;
        //$scope.viewportHeight = 0;

        //$scope.boundaryHeight = width;
        //$scope.boundaryWidth = height;

        //$scope.image = image == null ? 'a' : image;
        $scope.cropped = {
            source: 'a',
            image: null
        };

        $scope.loaded = false;

        setTimeout(function () {
            $scope.boundaryWidth = $(".modal-body").width()// - ($(".modal-body").width() / 100 * 25);

            $scope.boundaryHeight = $scope.boundaryWidth - ($scope.boundaryWidth / 100 * 50);

            $scope.viewportWidth = $scope.boundaryHeight - ($scope.boundaryHeight / 100 * 25);

            $scope.viewportHeight = $scope.boundaryHeight - ($scope.boundaryHeight / 100 * 25);

            $scope.loaded = true;
            // Assign blob to component when selecting a image
            $('#upload').on('change', function () {
                var input = this;

                if (input.files && input.files[0]) {
                    var reader = new FileReader();

                    reader.onload = function (e) {
                        // bind new Image to Component
                        $scope.cropped.source = e.target.result;
                        $scope.$apply();
                    }

                    reader.readAsDataURL(input.files[0]);
                }


            });
        }, 1000);

        $scope.ok = function () {
            $uibModalInstance.dismiss($scope.cropped);
        }

        $scope.Cancel = function () {
            $uibModalInstance.close();
        }
    });
}]);