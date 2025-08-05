
myApp.controller('CastController', function ($scope, $rootScope, $filter, $http, $uibModal, toaster, $ngConfirm, $stateParams, $state) {


    console.log('$stateParams', $stateParams);
    $scope.goToCharacter = function (charId) {
        $state.go('cast.character_details', { charId: charId });
    }
    $rootScope.get_char = () => {
        $scope.getcharacters();
        //$scope.getExtras();
        //$scope.getTalent();
        //$scope.getactor();



    }
    $scope.selectAll = () => {
        angular.forEach($scope.chars, function (item) {
            item.check = true;
        })
        $scope.checkSelected();
    }
    $scope.deleteSelected = () => {

        angular.forEach($scope.chars, function (item) {
            if (item.check) {
                $http.delete(root + 'api/ScenesandScript/DeleteChar/' + item.Id).then(function success(response) {
                    if (response.status === 200) {

                        $rootScope.GetOnBoarding();
                        $rootScope.get_char();
                        $scope.Character = { char: null };
                        $scope.char = null;
                    }

                }, function error() { });
            }
        });
        $scope.deselectAll();
        $scope.toggleMulti();
        $scope.checkSelected();
    }
    $scope.toggleMulti = () => {
        if ($scope.charCheck) {
            $scope.charCheck = false;
        } else {
            $scope.charCheck = true;
        }
        $scope.checkSelected();
        $scope.deselectAll();
    }
    $scope.DelAll = false;
    $scope.checkSelected = () => {
        var chk = false;
        angular.forEach($scope.chars, function (item) {
            if (item.check) {
            
                chk = true;
                return true;
            }
        })
        if (chk) {
            $scope.DelAll = true;
        } else {
            $scope.DelAll = false;
            return false;
        }
    }
    $scope.deselectAll = () => {
        angular.forEach($scope.chars, function (item) {
            item.check = false;
        })
        $scope.checkSelected();
}
    $scope.toggleCharCheck = (char) => {
        if (char.check) {
            char.check = false;
        } else {
            char.check = true;
        }
        $scope.checkSelected();
   
    }


    $scope.exselectAll = () => {
        angular.forEach($scope.extra, function (item) {
            item.check = true;
        })
        $scope.excheckSelected();
    }
    $scope.exdeleteSelected = () => {

        angular.forEach($scope.extra, function (item) {
            if (item.check) {
                $http.delete(root + 'api/ScenesandScript/DeleteExtra/' + item.Id).then(function success(response) {
                    if (response.status === 200) {

                        $rootScope.GetOnBoarding();
                        $rootScope.get_char();
                        $scope.Character = { char: null };
                        $scope.char = null;
                    }

                }, function error() { });
            }
        });
        $scope.exdeselectAll();
        $scope.extoggleMulti();
        $scope.excheckSelected();
    }
    $scope.extoggleMulti = () => {
        if ($scope.excharCheck) {
            $scope.excharCheck = false;
        } else {
            $scope.excharCheck = true;
        }
        $scope.excheckSelected();
        $scope.exdeselectAll();
    }
    $scope.exDelAll = false;
    $scope.excheckSelected = () => {
        var chk = false;
        angular.forEach($scope.extra, function (item) {
            if (item.check) {

                chk = true;
                return true;
            }
        })
        if (chk) {
            $scope.exDelAll = true;
        } else {
            $scope.exDelAll = false;
            return false;
        }
    }
    $scope.exdeselectAll = () => {
        angular.forEach($scope.extra, function (item) {
            item.check = false;
        })
        $scope.excheckSelected();
    }
    $scope.extoggleCharCheck = (char) => {
        if (char.check) {
            char.check = false;
        } else {
            char.check = true;
        }
        $scope.excheckSelected();

    }





    $scope.acselectAll = () => {
        angular.forEach($scope.Actor, function (item) {
            item.check = true;
        })
        $scope.accheckSelected();
    }
    $scope.acdeleteSelected = () => {

        angular.forEach($scope.Actor, function (item) {
            if (item.check) {
                $scope.DeleteActor(item);
            }
        });
        $scope.acdeselectAll();
        $scope.actoggleMulti();
        $scope.accheckSelected();
    }
    $scope.actoggleMulti = () => {
        if ($scope.accharCheck) {
            $scope.accharCheck = false;
        } else {
            $scope.accharCheck = true;
        }
        $scope.accheckSelected();
        $scope.acdeselectAll();
    }
    $scope.acDelAll = false;
    $scope.accheckSelected = () => {
        var chk = false;
        angular.forEach($scope.Actor, function (item) {
            if (item.check) {

                chk = true;
                return true;
            }
        })
        if (chk) {
            $scope.acDelAll = true;
        } else {
            $scope.acDelAll = false;
            return false;
        }
    }
    $scope.acdeselectAll = () => {
        angular.forEach($scope.Actor, function (item) {
            item.check = false;
        })
        $scope.accheckSelected();
    }
    $scope.actoggleCharCheck = (char) => {
        if (char.check) {
            char.check = false;
        } else {
            char.check = true;
        }
        $scope.accheckSelected();

    }



    $scope.tlselectAll = () => {
        angular.forEach($scope.Talent, function (item) {
            item.check = true;
        })
        $scope.tlcheckSelected();
    }
    $scope.tldeleteSelected = () => {

        angular.forEach($scope.Talent, function (item) {
            if (item.check) {
                $scope.DeleteTalent(item);
            }
        });
        $scope.tldeselectAll();
        $scope.tltoggleMulti();
        $scope.tlcheckSelected();
    }
    $scope.tltoggleMulti = () => {
        if ($scope.tlcharCheck) {
            $scope.tlcharCheck = false;
        } else {
            $scope.tlcharCheck = true;
        }
        $scope.tlcheckSelected();
        $scope.tldeselectAll();
    }
    $scope.tlDelAll = false;
    $scope.tlcheckSelected = () => {
        var chk = false;
        angular.forEach($scope.Talent, function (item) {
            if (item.check) {

                chk = true;
                return true;
            }
        })
        if (chk) {
            $scope.tlDelAll = true;
        } else {
            $scope.tlDelAll = false;
            return false;
        }
    }
    $scope.tldeselectAll = () => {
        angular.forEach($scope.Talent, function (item) {
            item.check = false;
        })
        $scope.tlcheckSelected();
    }
    $scope.tltoggleCharCheck = (char) => {
        if (char.check) {
            char.check = false;
        } else {
            char.check = true;
        }
        $scope.tlcheckSelected();

    }




    $scope.agselectAll = () => {
        angular.forEach($scope.agency, function (item) {
            item.check = true;
        })
        $scope.agcheckSelected();
    }
    $scope.agdeleteSelected = () => {

        angular.forEach($scope.agency, function (item) {
            if (item.check) {
                $scope.DeleteAgency(item);
            }
        });
        $scope.agdeselectAll();
        $scope.agtoggleMulti();
        $scope.agcheckSelected();
    }
    $scope.agtoggleMulti = () => {
        if ($scope.agcharCheck) {
            $scope.agcharCheck = false;
        } else {
            $scope.agcharCheck = true;
        }
        $scope.agcheckSelected();
        $scope.agdeselectAll();
    }
    $scope.agDelAll = false;
    $scope.agcheckSelected = () => {
        var chk = false;
        angular.forEach($scope.agency, function (item) {
            if (item.check) {

                chk = true;
                return true;
            }
        })
        if (chk) {
            $scope.agDelAll = true;
        } else {
            $scope.agDelAll = false;
            return false;
        }
    }
    $scope.agdeselectAll = () => {
        angular.forEach($scope.agency, function (item) {
            item.check = false;
        })
        $scope.agcheckSelected();
    }
    $scope.agtoggleCharCheck = (char) => {
        if (char.check) {
            char.check = false;
        } else {
            char.check = true;
        }
        $scope.agcheckSelected();

    }

    $scope.gotoActor = (id) => {
        $state.go('cast.actor_details', { charId: id })
    }

    $scope.gotoTalent = (id) => {
        $state.go('cast.talent_details', { charId: id })
    }
    $scope.charCheck = false;
    $scope.listview = true;
    $scope.toggleListView = () => {
        if ($scope.listview) {
            $scope.listview = false;
        } else {
            $scope.listview = true;
        }
    }


    $scope.excharCheck = false;
    $scope.exlistview = true;
    $scope.extoggleListView = () => {
        if ($scope.exlistview) {
            $scope.exlistview = false;
        } else {
            $scope.exlistview = true;
        }
    }
    $scope.accharCheck = false;
    $scope.aclistview = true;
    $scope.actoggleListView = () => {
        if ($scope.aclistview) {
            $scope.aclistview = false;
        } else {
            $scope.aclistview = true;
        }
    }



    $scope.tlcharCheck = false;
    $scope.tllistview = true;
    $scope.tltoggleListView = () => {
        if ($scope.tllistview) {
            $scope.tllistview = false;
        } else {
            $scope.tllistview = true;
        }
    }
    $scope.agcharCheck = false;
    $scope.aglistview = true;
    $scope.agtoggleListView = () => {
        if ($scope.aglistview) {
            $scope.aglistview = false;
        } else {
            $scope.aglistview = true;
        }
    }
    $scope.getDetails = () => {

        $scope.create_charbtn = false;

        $scope.Name = "";
        $scope.getcharacters();
        $scope.getExtras();

    }
    $scope.openFileViewer = function (file, allFiles) {
        //console.log(project);
        setTimeout(function () {
            $("#fileViewer").css('z-index', '10000000');
        }, 1000);
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/centralized-templates/files-viewer.html',
            controller: 'fileViewerCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                fileObj: function () {
                    return file;
                },
                allFiles: function () {
                    return allFiles;
                },
                projectUsersAndGroups: function () {
                    return $scope.projectUsersAndGroups
                },
                fileProfileLink: function () {
                    return 'scenesandscripts.scene_details.fileProfile'
                }
            }
        }).rendered.then(function () {
            $uibModalStack.getTop().value.modalDomEl.attr('id', "fileViewer");
        });
        modalInstance.result.then(function () {
        }, function (data) {
            console.log(data, 'from modal');
            //$scope.getProjects();
        });

    }
  
    $scope.Charsort = { name: false, index: false }
    $scope._sort = 1;
    $scope.sortByid = () => {
        if ($scope._sort === 1) {
            $scope.dropdown = -1
        }
        if ($scope.Charsort.index === true) {
            $scope.Charsort.index = false;
            $scope.chars.sort(function (a, b) {
                return a.Index - b.Index
            });
       
        } else {
            $scope.Charsort.index = true;
            $scope.chars.sort(function (a, b) {
                return b.Index - a.Index
            });
        }
      
        $scope.Charsort.name = false;
        $scope._sort = 1;
    }
    
    $scope.sortByname = () => {
        if ($scope._sort === 2) {
            $scope.dropdown = -1
        }
        if ($scope.Charsort.name === true) {
            $scope.Charsort.name = false;
            $scope.chars.sort(function (a, b) {
                var val = (a.Name) > (b.Name)?1:-1
                return val
            });
            
        } else {
            $scope.Charsort.name = true;
            $scope.chars.sort(function (a, b) {
                var val = (a.Name) < (b.Name) ? 1 : -1

                return val
            });
        }
 
        $scope.Charsort.index = false;
        $scope._sort = 2;
    }




    $scope.Exsort = { name: false, index: false }
    $scope.ex_sort = 1;
    $scope.exsortByid = () => {
        if ($scope.ex_sort === 1) {
            $scope.dropdown = -1
        }
        if ($scope.Exsort.index === true) {
            $scope.Exsort.index = false;
            $scope.extra.sort(function (a, b) {
                return a.Index - b.Index
            });
          
        } else {
            $scope.Exsort.index = true;
            $scope.extra.sort(function (a, b) {
                return b.Index - a.Index
            });
        }
        $scope.Exsort.name = false;
        $scope.ex_sort = 1;
    
    }

    $scope.exsortByname = () => {
        if ($scope.ex_sort === 2) {
            $scope.dropdown = -1
        }
        if ($scope.Exsort.name === true) {
            $scope.Exsort.name = false;
            $scope.extra.sort(function (a, b) {
                var val = (a.Name) > (b.Name) ? 1 : -1
                return val
            });
        } else {
            $scope.Exsort.name = true;
            $scope.extra.sort(function (a, b) {
                var val = (a.Name) < (b.Name) ? 1 : -1

                return val
            });
        }
        $scope.Exsort.index = false;
        $scope.ex_sort = 2;
     
    }


    $scope.Acsort = { name: false, index: false }
    $scope.ac_sort = 1;
    $scope.acsortByid = () => {
        if ($scope.ac_sort === 1) {
            $scope.dropdown = -1
        }
        if ($scope.Acsort.index === true) {
            $scope.Acsort.index = false;
            $scope.Actor.sort(function (a, b) {
                return a.Id - b.Id
            });
         
        } else {
            $scope.Acsort.index = true;
            $scope.Actor.sort(function (a, b) {
                return b.Id - a.Id
            });
        }
        $scope.Acsort.name = false;
        $scope.ac_sort = 1;
     
    }

    $scope.acsortByname = () => {
        if ($scope.ac_sort === 2) {
            $scope.dropdown = -1
        }
        if ($scope.Acsort.name === true) {
            $scope.Acsort.name = false;
            $scope.Actor.sort(function (a, b) {
                var val = (a.Name) > (b.Name) ? 1 : -1
                return val
            });
          
        } else {
            $scope.Acsort.name = true;
            $scope.Actor.sort(function (a, b) {
                var val = (a.Name) < (b.Name) ? 1 : -1

                return val
            });
        }
   
        $scope.Acsort.index = false;
        $scope.ac_sort = 2;
    }



    $scope.Tlsort = { name: false, index: false }
    $scope.tl_sort = 1;
    $scope.tlsortByid = () => {
        if ($scope.tl_sort === 1) {
            $scope.dropdown = -1
        }
        if ($scope.Tlsort.index === true) {
            $scope.Tlsort.index = false;
            $scope.Talent.sort(function (a, b) {
                return a.Id - b.Id
            });
         
        } else {
            $scope.Tlsort.index = true;
            $scope.Talent.sort(function (a, b) {
                return b.Id - a.Id
            });
        }
        $scope.Tlsort.name = false;
        $scope.tl_sort = 1;
 
    }

    $scope.tlsortByname = () => {
        if ($scope.tl_sort ===2) {
            $scope.dropdown = -1
        }
        if ($scope.Tlsort.name === true) {
            $scope.Tlsort.name = false;
            $scope.Talent.sort(function (a, b) {
                var val = (a.Name) > (b.Name) ? 1 : -1
                return val
            });
           
        } else {
            $scope.Tlsort.name = true;
            $scope.Talent.sort(function (a, b) {
                var val = (a.Name) < (b.Name) ? 1 : -1

                return val
            });
        }
        $scope.Tlsort.index = false;
        $scope.tl_sort = 2;
    }




    $scope.Agsort = { name: false, index: false }
    $scope.ag_sort = 1;
    $scope.agsortByid = () => {
        if ($scope.ag_sort === 1) {
            $scope.dropdown = -1
        }
        if ($scope.Agsort.index === true) {
            $scope.Agsort.index = false;
            $scope.agency.sort(function (a, b) {
                return a.Agency.Id - b.Agency.Id
            });
          
        } else {
            $scope.Agsort.index = true;
            $scope.agency.sort(function (a, b) {
                return b.Agency.Id - a.Agency.Id
            });
        }
     
        $scope.Agsort.name = false;
        $scope.ag_sort = 1;
    }

    $scope.agsortByname = () => {
        if ($scope.ag_sort === 2) {
            $scope.dropdown = -1
        }
        if ($scope.Agsort.name === true) {
            $scope.Agsort.name = false;
            $scope.agency.sort(function (a, b) {
                var val = (a.Agency.Name) > (b.Agency.Name) ? 1 : -1
                return val
            });
          
        } else {
            $scope.Agsort.name = true;
            $scope.agency.sort(function (a, b) {
                var val = (a.Agency.Name) < (b.Agency.Name) ? 1 : -1

                return val
            });
        }
  
        $scope.Agsort.index = false;
        $scope.ag_sort = 2;
    }
    var projectId = parseInt($stateParams.id);
    //Characters
    $scope.dropdown = -1;
    $scope.ShowDropDown = (id, type) => {
        if ($scope.dropdown === -1  || id+type != $scope.dropdown) {
            $scope.dropdown = id + type;
        } else {
            $scope.dropdown = -1;
        }
    }
    $(window).click(function () {
        setTimeout(function () {
            $scope.$apply(function () {
                $scope.dropdown = -1;
            });
        });
    });

    $scope.Character = { char: null };
    $scope.Merge = (char,type) => {
        $scope.char = char;
        $scope.Character.type=type
        if (type == 0) {
            $scope.filtered_char = $scope.chars.filter(function (item) {
                if (item.Id != char.Id) {
                    return item;
                }
            });
        } else {
            $scope.filtered_char = $scope.extra.filter(function (item) {
                if (item.Id != char.Id) {
                    return item;
                }
            });
        }
        $('#myModal').modal('show');
    }
    $scope.MergeChar = (chid2, chid,type) => {
        if (type == 0) {
            $http.get(root + 'api/ScenesandScript/MergeCharacter/' + chid + '/' + chid2).then(function success(response) {

                $http.delete(root + 'api/ScenesandScript/DeleteChar/' + chid).then(function success(response) {
                    if (response.status === 200) {

                        $rootScope.GetOnBoarding();
                        $rootScope.get_char();
                        $scope.Character = { char: null };
                        $scope.char = null;
                    }

                }, function error() { });

            }, function error() { });
        } else {
            $http.get(root + 'api/ScenesandScript/MergeExtra/' + chid + '/' + chid2).then(function success(response) {

                $http.delete(root + 'api/ScenesandScript/DeleteExtra/' + chid).then(function success(response) {
                    if (response.status === 200) {

                        $rootScope.GetOnBoarding();
                        $rootScope.get_char();
                        $scope.Character = { char: null };
                        $scope.char = null;
                    }

                }, function error() { });

            }, function error() { });
        }
    }
    $scope.getcharacters = async function() {
      await  $http.get(root + 'api/ScenesandScript/GetCharacters?projectid=' + projectId).then(function success(response) {

            console.log(response);
          $scope.chars = response.data;
          $scope.chars1 = $scope.chars;
            //$scope.chars = angular.forEach($scope.chars, function (item) {
                //$http.get(root + 'api/ScenesandScript/getCharFiles/' + parseInt(item.Id) + '/' + $scope.filesPaging.page + '/' + $scope.filesPaging.size).then(resp => {
                //    $scope.defaultImage = resp.data.FileId;
                //    if (resp.data.list.length > 0) {
                //        $scope.files = resp.data.list;
                      
                //        angular.forEach($scope.files, function (item1) {
                //            if (item1.Default === true) {
                //                item.Default = item1;
                //                item.HasFile = true;
                //                item.file = item1;

                //            }
                //        });

                //    } 
                //}, err => {
                //});
           
            //    return item;
            //});

      }, function error() { });
        $scope.getCharActor();
    }
    $scope.srchbtn = false;
    $scope.checksearch = () => {
        if ($scope.srchbtn) {
            return 'fa fa-times';
        } else {

            return 'fa fa-search';
        }
    }
    $scope.show_search = () => {
        if ($scope.srchbtn) {
            $scope.srchbtn = false;
            $scope.chars = $scope.chars1;

        } else {
            $scope.srchbtn = true;
        }
    }
    $scope.search = (string) => {
        if (string == null || string == "undefined") {

            $scope.chars = $scope.chars1;
        } else {
            output = []
            angular.forEach($scope.chars1, function (item) {

                if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                    output.push(item);
                }
            });

            $scope.chars = output;
        }
    }
    $scope.exsrchbtn = false;
    $scope.exchecksearch = () => {
        if ($scope.exsrchbtn) {
            return 'fa fa-times';
        } else {

            return 'fa fa-search';
        }
    }
    $scope.exshow_search = () => {
        if ($scope.exsrchbtn) {
            $scope.exsrchbtn = false;
            $scope.extra = $scope.extra1;
        } else {
            $scope.exsrchbtn = true;
        }
    }
    $scope.exsearch = (string) => {
        if (string == null || string == "undefined") {

            $scope.extra = $scope.extra1;
        } else {
            output = []
            angular.forEach($scope.extra1, function (item) {

                if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                    output.push(item);
                }
            });

            $scope.extra = output;
        }
    }


    $scope.acsrchbtn = false;
    $scope.acchecksearch = () => {
        if ($scope.acsrchbtn) {
            return 'fa fa-times';
        } else {

            return 'fa fa-search';
        }
    }
    $scope.acshow_search = () => {
        if ($scope.acsrchbtn) {
            $scope.acsrchbtn = false;

            $scope.Actor = $scope.Actor1;
        } else {
            $scope.acsrchbtn = true;
        }
    }
    $scope.acsearch = (string) => {
        if (string == null || string == "undefined") {

            $scope.Actor = $scope.Actor1;
        } else {
            output = []
            angular.forEach($scope.Actor1, function (item) {

                if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                    output.push(item);
                }
            });

            $scope.Actor = output;
        }
    }



    $scope.agsrchbtn = false;
    $scope.agchecksearch = () => {
        if ($scope.agsrchbtn) {
            return 'fa fa-times';
        } else {

            return 'fa fa-search';
        }
    }
    $scope.agshow_search = () => {
        if ($scope.agsrchbtn) {
            $scope.agsrchbtn = false;
            $scope.agency = $scope.agency1;
        } else {
            $scope.agsrchbtn = true;
        }
    }
    $scope.agsearch = (string) => {
        if (string == null || string == "undefined") {

            $scope.agency = $scope.agency1;
        } else {
            output = []
            angular.forEach($scope.agency1, function (item) {

                if (item.Agency.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                    output.push(item);
                }
            });

            $scope.agency = output;
        }
    }


    $scope.tlsrchbtn = false;
    $scope.tlchecksearch = () => {
        if ($scope.tlsrchbtn) {
            return 'fa fa-times';
        } else {

            return 'fa fa-search';
        }
    }
    $scope.tlshow_search = () => {
        if ($scope.tlsrchbtn) {
            $scope.tlsrchbtn = false;
            $scope.Talent = $scope.Talent1;
        } else {
            $scope.tlsrchbtn = true;
        }
    }
    $scope.tlsearch = (string) => {
        if (string == null || string == "undefined") {

            $scope.Talent = $scope.Talent1;
        } else {
            output = []
            angular.forEach($scope.Talent1, function (item) {

                if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                    output.push(item);
                }
            });

            $scope.Talent = output;
        }
    }

    $scope.getCharActor = () => {
        $scope.chars = angular.forEach($scope.chars, function (item) {
            $http.get(root + 'api/ScenesandScript/GetCharacterTalentByChId?charId=' + parseInt(item.Id)).then(function (response) {
                var data = response.data;
                if (data.length > 0) {
                    $scope.getactor();
                    var arrayWithIds = [];
                    var arrayWithTalentIds = [];
                    angular.forEach(data, function (x) {
                        console.log(x);
                        if (x.ActorId) {
                            arrayWithIds.push(x.ActorId);
                        } else {
                            arrayWithTalentIds.push(x.TalentId);
                        }
                    });
                    item.charActor = $scope.Actor.filter(function (actor) {
                        var present = arrayWithIds.indexOf(actor.Id) != -1;
                        if (present) {
                            actor.actor = true;
                            return actor;
                        }
                    });
                    if (item.charActor.length <= 0) {
                        item.charActor = $scope.Talent.filter(function (actor) {
                            var present = arrayWithTalentIds.indexOf(actor.Id) != -1;
                            if (present) {
                                actor.actor = false;
                                return actor;
                            }
                        });
                    }
                }
            }, error => { });
        });
        console.log($scope.chars, "charActor");
    }
    $scope.getExtraActor = () => {
        $scope.extra = angular.forEach($scope.extra, function (item) {
            $http.get(root + 'api/ScenesandScript/GetCharacterTalentByExId?exId=' + parseInt(item.Id)).then(function (response) {
                var data = response.data;
                if (data.length > 0) {
                    $scope.getactor();
                    var arrayWithIds = [];
                    var arrayWithTalentIds = [];
                    angular.forEach(data, function (x) {
                        console.log(x);
                        if (x.ActorId) {
                            arrayWithIds.push(x.ActorId);
                        } else {
                            arrayWithTalentIds.push(x.TalentId);
                        }
                    });
                    item.charActor = $scope.Actor.filter(function (actor) {
                        var present = arrayWithIds.indexOf(actor.Id) != -1;
                        if (present) {
                            actor.actor = true;
                            return actor;
                        }
                    });
                    if (item.charActor.length <= 0) {
                        item.charActor = $scope.Talent.filter(function (actor) {
                            var present = arrayWithTalentIds.indexOf(actor.Id) != -1;
                            if (present) {
                                actor.actor = false;
                                return actor;
                            }
                        });
                    }
                }
            }, error => { });
        });
        console.log($scope.extra, "Extra");
    }
    $scope.filesPaging = {
        page: 1,
        size: 1200
    }
    $rootScope.getAgency = () => {
        $http.get(root + 'api/ScenesandScript/GetAgency?projectid=' + projectId).then(function (response) {
            if (response.status === 200) {
                $scope.agency = response.data;
                console.log($scope.agency);
                //$scope.agency.forEach(function (item) {
                //    $http.get(root + 'api/ScenesandScript/GetAgencyFiles/' + parseInt(item.Agency.Id) + '/' + $scope.filesPaging.page + '/' + $scope.filesPaging.size).then(resp => {
                //        $scope.defaultImage = resp.data.FileId;
                //        $scope.files = resp.data.list;
                //        item.FileCount = $scope.files.length;
                //        if (item.FileCount > 0) {
                //            angular.forEach($scope.files, function (item1) {
                //                if (item1.Default === true) {
                //                    item.Default = item1;
                //                    item.HasFile = true;
                //                    item.file = item1;

                //                }
                //            });
                //        } else {
                //            item.HasFile = false;
                //        }
                //    }, err => {
                //    });
                //});
                $scope.agency1 = $scope.agency
            }
        }, function (error) { });
    }
    $rootScope.getAgency();
    $scope.Actor = [];
    $rootScope.getTalent = () => {
        $http.get(root + 'api/ScenesandScript/GetTalent?projectid=' + projectId).then(function (response) {
            if (response.status === 200) {
                $scope.Talent = response.data;
                console.log($scope.Talent);
                //$scope.Talent.forEach(function (item) {
                //    $http.get(root + 'api/ScenesandScript/GetTalentFiles/' + parseInt(item.Id) + '/' + $scope.filesPaging.page + '/' + $scope.filesPaging.size).then(resp => {
                //        $scope.defaultImage = resp.data.FileId;
                //        $scope.files = resp.data.list;
                //        item.FileCount = $scope.files.length;
                //        if (item.FileCount > 0) {
                //            angular.forEach($scope.files, function (item1) {
                //                if (item1.Default === true) {
                //                    item.Default = item1;
                //                    item.HasFile = true;
                //                    item.file = item1;

                //                }
                //            });
                //        } else {
                //            item.HasFile = false;
                //        }
                //    }, err => {
                //    });
                //});
                $scope.Talent1 = $scope.Talent;
            }
        }, function (error) { });
    }
    $rootScope.getTalent();
    $rootScope.getactor = () => {
        $http.get(root + 'api/ScenesandScript/GetActor?projectid=' + projectId).then(function (response) {
            if (response.status === 200) {
                $scope.Actor = response.data;
                
                //$scope.Actor.forEach(function (item) {
                //    $http.get(root + 'api/ScenesandScript/GetActorFiles/' + parseInt(item.Id) + '/' + $scope.filesPaging.page + '/' + $scope.filesPaging.size).then(resp => {
                //        $scope.defaultImage = resp.data.FileId;
                //        $scope.files = resp.data.list;
                //        item.FileCount = $scope.files.length;
                //        if (item.FileCount > 0) {
                //            console.log($scope.files);
                //            angular.forEach($scope.files, function (item1) {
                //                if (item1.Default === true) {
                //                    item.Default = item1;
                //                    item.HasFile = true;
                //                    item.file = item1;

                //                }
                //            });
                //            //item.FileId = $scope.Files[0].FileId;
                //        } else {
                //            item.HasFile = false;
                //        }
                //    }, err => {
                //    });
                //});
                $scope.Actor1 = $scope.Actor;
            }
        }, function (error) { });
    }
    $rootScope.getactor();
    $scope.charIndex=-1
    $scope.ShowDel = (id, type) => {
        $scope.charIndex = id + type;
    }
    $scope.hideDel = () => {
        $scope.charIndex = -1
    }
    $scope.UpdateTalent = (Talent) => {
    
        $http.post(root + 'api/scenesandscript/CreateTalent/', Talent).then(function (resp) {
            if (resp.status === 200) {
                $rootScope.getTalent();
            }
        }, function error() { });
    }
    $scope.UpdateActor = (Actor) => {
    
        $http.post(root + 'api/scenesandscript/CreateActor/', Actor).then(function (resp) {
            if (resp.status === 200) {
                $rootScope.getactor();
            }
        }, function error() { });
    }
    $scope.DeleteTalent = (Talent) => {
        Talent.Is_deleted = true;
        $http.post(root + 'api/scenesandscript/CreateTalent/', Talent).then(function (resp) {
            if (resp.status === 200) {
                $rootScope.getTalent();
            }
        }, function error() { });
    }
    $scope.DeleteActor = (Actor) => {
        Actor.Is_deleted = true;
        $http.post(root + 'api/scenesandscript/CreateActor/', Actor).then(function (resp) {
            if (resp.status === 200) {
                $rootScope.getactor();
            }
        }, function error() { });
    }
    $scope.UpdateAgency = (Agency) => {
        var agencyDto = {
            "Agency":Agency.Agency,
            "AgencyContact": Agency.AgencyContact
        }
        console.log($scope.agencyDto);
        $http.post(root + 'api/scenesandscript/CreateAgency/', agencyDto).then(function (resp) {
            if (resp.status === 200) {
                $rootScope.getAgency();
            }
        }, function error() { });
    }
    $scope.DeleteAgency = (Agency) => {
        Agency.Agency.Is_Deleted = true;
        var agencyDto = {
            "Agency": Agency.Agency,
            "AgencyContact": Agency.AgencyContact
        }
        console.log($scope.agencyDto);
        $http.post(root + 'api/scenesandscript/CreateAgency/', agencyDto).then(function (resp) {
            if (resp.status === 200) {
                $rootScope.getAgency();
            }
        }, function error() { });
    }

    $scope.editModes = {
        Character: false,
        Extra: false
    }
    $scope.create_char = () => {
        if (!$scope.editModes.Character) {
            $scope.editModes.Character = true;
            $scope.chars.push({
                Id: 0,
                Name: '',

                ProjectId: projectId
            });
            setTimeout(function () {
                document.getElementById('txtCharNameNew_0').focus();
            }, 100);// 
        }
    }



    $scope.DeleteChar = (id) => {
        $http.delete(root + 'api/ScenesandScript/DeleteChar/' + id).then(function success(response) {
            if (response.status === 200) {
                $scope.chars = $scope.chars.filter(function (item) {

                    if (item.Id != id) {
                        return item;
                    }
                });
                $rootScope.GetOnBoarding();
            }

        }, function error() { });
    }

    $scope.tidyup = () => {
        $scope.chars.forEach(function (char) {
            $scope.DeleteChar(char.Id);

        });

    }
    $scope.updateChar = (char) => {
        $scope.editModes.Character = false;
        if (char.Name) {
            var name = char.Name.replace(/&nbsp;/g, "");
            var newch = {
                "Id": char.Id,
                "Name": name,
                "Index": parseInt(char.Index),
                "Project_Id": projectId
            }
            $http.post(root + 'api/ScenesandScript/CreateChar', newch).then(
                function success(resp) {
                    if (resp.status === 200) {
                        char.Id = resp.data.Id;
                        char.Name = resp.data.Name;
                        char.Index = resp.data.Index;
                        $scope.getcharacters();

                    } else {

                        $scope.chars.splice(-1, 1);
                    }
                    $rootScope.GetOnBoarding();
                }, function error() { });
        } else {
            if (char.Id == 0) {
                $scope.chars = $scope.chars.filter(function (obj) {
                    return obj.Id !== char.Id;
                });
            }
        }
    }

    //Extras


    $scope.getExtras = async function () {
     await   $http.get(root + 'api/ScenesandScript/GetExtras?projectid=' + projectId).then(function success(response) {

            console.log(response);
            $scope.extra = response.data;
            $scope.extra1 = response.data;
            //$scope.extra = angular.forEach($scope.extra, function (item) {
            //    $http.get(root + 'api/ScenesandScript/getExtraFiles/' + parseInt(item.Id) + '/' + $scope.filesPaging.page + '/' + $scope.filesPaging.size).then(resp => {
            //        $scope.defaultImage = resp.data.FileId;
            //        if (resp.data.list.length > 0) {
            //            $scope.files = resp.data.list;

            //            angular.forEach($scope.files, function (item1) {
            //                if (item1.Default === true) {
            //                    item.Default = item1;
            //                    item.HasFile = true;
            //                    item.file = item1;

            //                }
            //            });
            //        }
            //    }, err => {
            //    });
            //    return item;
            //});

     }, function error() { });
        $scope.getExtraActor();
    }



    $scope.create_ex = () => {
        if (!$scope.editModes.Extra) {
            $scope.editModes.Extra = true;
            $scope.extra.push({
                Id: 0,
                Name: '',
                ProjectId: projectId
            });
            setTimeout(function () {
                document.getElementById('txtNewExtraName_0').focus();
            }, 100);// 
        }

    }

    $scope.updateEx = (char) => {
        $scope.editModes.Extra = false;
        if (char.Name) {
            var name = char.Name.replace(/&nbsp;/g, "");
            var newch = {
                "Id": char.Id,
                "Name": name,
                "Project_Id": projectId,
                "Index": parseInt(char.Index)
            }
            $http.post(root + 'api/ScenesandScript/Createext', newch).then(
                function success(resp) {
                    if (resp.status === 200) {

                        char.Id = resp.data.Id;
                        char.Name = resp.data.Name;
                        char.Index = resp.data.Index;
                        $scope.getExtras();

                    }
                }, function error() { });
        } else {
            if (char.Id == 0) {
                $scope.extra = $scope.extra.filter(function (obj) {
                    return obj.Id !== char.Id;
                });
            }
        }
    }

    $scope.DeleteEx = (id) => {
        $http.delete(root + 'api/ScenesandScript/DeleteExtra/' + id).then(function success(response) {
            if (response.status === 200) {
                $scope.extra = $scope.extra.filter(function (item) {

                    if (item.Id != id) {
                        return item;
                    }
                });

            }

        }, function error() { });
    }




    //Create-Agency
    var modalInstance = null;
    $scope.create_agency = function () {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/Cast/create-agency.html',
            controller: 'CreateAgencyCtrl',
            size: 'md',
            backdrop: 'static',
            resolve: {
                title: function () {
                    return 'Create Agency ';
                },
                projectItem: function () {
                    return null;
                },
                projectId: function () {
                    return projectId;
                }, Name: function () {
                    return null;
                }

            }
        });
        modalInstance.result.then(function () {

        }, function (data) {

        });
    }  //Create-Talent
     modalInstance = null;
    $scope.create_talent = function () {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/Cast/create-talent.html',
            controller: 'CreateTalentCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                title: function () {
                    return 'Create Talent ';
                },
                projectItem: function () {
                    return null;
                },
                projectId: function () {
                    return projectId;
                }

            }
        });
        modalInstance.result.then(function () {

        }, function (data) {

        });
    }
    modalInstance = null;
    $scope.create_actor = function () {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/Cast/create-actor.html',
            controller: 'CreateActorCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                title: function () {
                    return 'Create Actor ';
                },
                projectItem: function () {
                    return null;
                },
                projectId: function () {
                    return projectId;
                }

            }
        });
        modalInstance.result.then(function () {

        }, function (data) {

        });
    }
    $scope.getDetails();

});
myApp.controller('CreateAgencyCtrl', function ($scope, $rootScope, $filter, $http, $uibModal, $uibModalInstance, $timeout, toaster, $ngConfirm, $stateParams, $state, title, projectId,Name) {
    angular.element(document).ready(function () {
        $scope.title = title;

        $scope.ProjectId = projectId;
        $scope.files = [];
        $scope.Agency = { Country: null };
        $scope.Agency.Name = Name;
        $scope.cancel = function () {
            $uibModalInstance.dismiss();
        }
        $scope.countries = [
            { name: 'Afghanistan', code: 'AF' },
            { name: 'Åland Islands', code: 'AX' },
            { name: 'Albania', code: 'AL' },
            { name: 'Algeria', code: 'DZ' },
            { name: 'American Samoa', code: 'AS' },
            { name: 'Andorra', code: 'AD' },
            { name: 'Angola', code: 'AO' },
            { name: 'Anguilla', code: 'AI' },
            { name: 'Antarctica', code: 'AQ' },
            { name: 'Antigua and Barbuda', code: 'AG' },
            { name: 'Argentina', code: 'AR' },
            { name: 'Armenia', code: 'AM' },
            { name: 'Aruba', code: 'AW' },
            { name: 'Australia', code: 'AU' },
            { name: 'Austria', code: 'AT' },
            { name: 'Azerbaijan', code: 'AZ' },
            { name: 'Bahamas', code: 'BS' },
            { name: 'Bahrain', code: 'BH' },
            { name: 'Bangladesh', code: 'BD' },
            { name: 'Barbados', code: 'BB' },
            { name: 'Belarus', code: 'BY' },
            { name: 'Belgium', code: 'BE' },
            { name: 'Belize', code: 'BZ' },
            { name: 'Benin', code: 'BJ' },
            { name: 'Bermuda', code: 'BM' },
            { name: 'Bhutan', code: 'BT' },
            { name: 'Bolivia', code: 'BO' },
            { name: 'Bosnia and Herzegovina', code: 'BA' },
            { name: 'Botswana', code: 'BW' },
            { name: 'Bouvet Island', code: 'BV' },
            { name: 'Brazil', code: 'BR' },
            { name: 'British Indian Ocean Territory', code: 'IO' },
            { name: 'Brunei Darussalam', code: 'BN' },
            { name: 'Bulgaria', code: 'BG' },
            { name: 'Burkina Faso', code: 'BF' },
            { name: 'Burundi', code: 'BI' },
            { name: 'Cambodia', code: 'KH' },
            { name: 'Cameroon', code: 'CM' },
            { name: 'Canada', code: 'CA' },
            { name: 'Cape Verde', code: 'CV' },
            { name: 'Cayman Islands', code: 'KY' },
            { name: 'Central African Republic', code: 'CF' },
            { name: 'Chad', code: 'TD' },
            { name: 'Chile', code: 'CL' },
            { name: 'China', code: 'CN' },
            { name: 'Christmas Island', code: 'CX' },
            { name: 'Cocos (Keeling) Islands', code: 'CC' },
            { name: 'Colombia', code: 'CO' },
            { name: 'Comoros', code: 'KM' },
            { name: 'Congo', code: 'CG' },
            { name: 'Congo, The Democratic Republic of the', code: 'CD' },
            { name: 'Cook Islands', code: 'CK' },
            { name: 'Costa Rica', code: 'CR' },
            { name: 'Cote D\'Ivoire', code: 'CI' },
            { name: 'Croatia', code: 'HR' },
            { name: 'Cuba', code: 'CU' },
            { name: 'Cyprus', code: 'CY' },
            { name: 'Czech Republic', code: 'CZ' },
            { name: 'Denmark', code: 'DK' },
            { name: 'Djibouti', code: 'DJ' },
            { name: 'Dominica', code: 'DM' },
            { name: 'Dominican Republic', code: 'DO' },
            { name: 'Ecuador', code: 'EC' },
            { name: 'Egypt', code: 'EG' },
            { name: 'El Salvador', code: 'SV' },
            { name: 'Equatorial Guinea', code: 'GQ' },
            { name: 'Eritrea', code: 'ER' },
            { name: 'Estonia', code: 'EE' },
            { name: 'Ethiopia', code: 'ET' },
            { name: 'Falkland Islands (Malvinas)', code: 'FK' },
            { name: 'Faroe Islands', code: 'FO' },
            { name: 'Fiji', code: 'FJ' },
            { name: 'Finland', code: 'FI' },
            { name: 'France', code: 'FR' },
            { name: 'French Guiana', code: 'GF' },
            { name: 'French Polynesia', code: 'PF' },
            { name: 'French Southern Territories', code: 'TF' },
            { name: 'Gabon', code: 'GA' },
            { name: 'Gambia', code: 'GM' },
            { name: 'Georgia', code: 'GE' },
            { name: 'Germany', code: 'DE' },
            { name: 'Ghana', code: 'GH' },
            { name: 'Gibraltar', code: 'GI' },
            { name: 'Greece', code: 'GR' },
            { name: 'Greenland', code: 'GL' },
            { name: 'Grenada', code: 'GD' },
            { name: 'Guadeloupe', code: 'GP' },
            { name: 'Guam', code: 'GU' },
            { name: 'Guatemala', code: 'GT' },
            { name: 'Guernsey', code: 'GG' },
            { name: 'Guinea', code: 'GN' },
            { name: 'Guinea-Bissau', code: 'GW' },
            { name: 'Guyana', code: 'GY' },
            { name: 'Haiti', code: 'HT' },
            { name: 'Heard Island and Mcdonald Islands', code: 'HM' },
            { name: 'Holy See (Vatican City State)', code: 'VA' },
            { name: 'Honduras', code: 'HN' },
            { name: 'Hong Kong', code: 'HK' },
            { name: 'Hungary', code: 'HU' },
            { name: 'Iceland', code: 'IS' },
            { name: 'India', code: 'IN' },
            { name: 'Indonesia', code: 'ID' },
            { name: 'Iran, Islamic Republic Of', code: 'IR' },
            { name: 'Iraq', code: 'IQ' },
            { name: 'Ireland', code: 'IE' },
            { name: 'Isle of Man', code: 'IM' },
            { name: 'Israel', code: 'IL' },
            { name: 'Italy', code: 'IT' },
            { name: 'Jamaica', code: 'JM' },
            { name: 'Japan', code: 'JP' },
            { name: 'Jersey', code: 'JE' },
            { name: 'Jordan', code: 'JO' },
            { name: 'Kazakhstan', code: 'KZ' },
            { name: 'Kenya', code: 'KE' },
            { name: 'Kiribati', code: 'KI' },
            { name: 'Korea, Democratic People\'s Republic of', code: 'KP' },
            { name: 'Korea, Republic of', code: 'KR' },
            { name: 'Kuwait', code: 'KW' },
            { name: 'Kyrgyzstan', code: 'KG' },
            { name: 'Lao People\'s Democratic Republic', code: 'LA' },
            { name: 'Latvia', code: 'LV' },
            { name: 'Lebanon', code: 'LB' },
            { name: 'Lesotho', code: 'LS' },
            { name: 'Liberia', code: 'LR' },
            { name: 'Libyan Arab Jamahiriya', code: 'LY' },
            { name: 'Liechtenstein', code: 'LI' },
            { name: 'Lithuania', code: 'LT' },
            { name: 'Luxembourg', code: 'LU' },
            { name: 'Macao', code: 'MO' },
            { name: 'Macedonia, The Former Yugoslav Republic of', code: 'MK' },
            { name: 'Madagascar', code: 'MG' },
            { name: 'Malawi', code: 'MW' },
            { name: 'Malaysia', code: 'MY' },
            { name: 'Maldives', code: 'MV' },
            { name: 'Mali', code: 'ML' },
            { name: 'Malta', code: 'MT' },
            { name: 'Marshall Islands', code: 'MH' },
            { name: 'Martinique', code: 'MQ' },
            { name: 'Mauritania', code: 'MR' },
            { name: 'Mauritius', code: 'MU' },
            { name: 'Mayotte', code: 'YT' },
            { name: 'Mexico', code: 'MX' },
            { name: 'Micronesia, Federated States of', code: 'FM' },
            { name: 'Moldova, Republic of', code: 'MD' },
            { name: 'Monaco', code: 'MC' },
            { name: 'Mongolia', code: 'MN' },
            { name: 'Montserrat', code: 'MS' },
            { name: 'Morocco', code: 'MA' },
            { name: 'Mozambique', code: 'MZ' },
            { name: 'Myanmar', code: 'MM' },
            { name: 'Namibia', code: 'NA' },
            { name: 'Nauru', code: 'NR' },
            { name: 'Nepal', code: 'NP' },
            { name: 'Netherlands', code: 'NL' },
            { name: 'Netherlands Antilles', code: 'AN' },
            { name: 'New Caledonia', code: 'NC' },
            { name: 'New Zealand', code: 'NZ' },
            { name: 'Nicaragua', code: 'NI' },
            { name: 'Niger', code: 'NE' },
            { name: 'Nigeria', code: 'NG' },
            { name: 'Niue', code: 'NU' },
            { name: 'Norfolk Island', code: 'NF' },
            { name: 'Northern Mariana Islands', code: 'MP' },
            { name: 'Norway', code: 'NO' },
            { name: 'Oman', code: 'OM' },
            { name: 'Pakistan', code: 'PK' },
            { name: 'Palau', code: 'PW' },
            { name: 'Palestinian Territory, Occupied', code: 'PS' },
            { name: 'Panama', code: 'PA' },
            { name: 'Papua New Guinea', code: 'PG' },
            { name: 'Paraguay', code: 'PY' },
            { name: 'Peru', code: 'PE' },
            { name: 'Philippines', code: 'PH' },
            { name: 'Pitcairn', code: 'PN' },
            { name: 'Poland', code: 'PL' },
            { name: 'Portugal', code: 'PT' },
            { name: 'Puerto Rico', code: 'PR' },
            { name: 'Qatar', code: 'QA' },
            { name: 'Reunion', code: 'RE' },
            { name: 'Romania', code: 'RO' },
            { name: 'Russian Federation', code: 'RU' },
            { name: 'Rwanda', code: 'RW' },
            { name: 'Saint Helena', code: 'SH' },
            { name: 'Saint Kitts and Nevis', code: 'KN' },
            { name: 'Saint Lucia', code: 'LC' },
            { name: 'Saint Pierre and Miquelon', code: 'PM' },
            { name: 'Saint Vincent and the Grenadines', code: 'VC' },
            { name: 'Samoa', code: 'WS' },
            { name: 'San Marino', code: 'SM' },
            { name: 'Sao Tome and Principe', code: 'ST' },
            { name: 'Saudi Arabia', code: 'SA' },
            { name: 'Senegal', code: 'SN' },
            { name: 'Serbia and Montenegro', code: 'CS' },
            { name: 'Seychelles', code: 'SC' },
            { name: 'Sierra Leone', code: 'SL' },
            { name: 'Singapore', code: 'SG' },
            { name: 'Slovakia', code: 'SK' },
            { name: 'Slovenia', code: 'SI' },
            { name: 'Solomon Islands', code: 'SB' },
            { name: 'Somalia', code: 'SO' },
            { name: 'South Africa', code: 'ZA' },
            { name: 'South Georgia and the South Sandwich Islands', code: 'GS' },
            { name: 'Spain', code: 'ES' },
            { name: 'Sri Lanka', code: 'LK' },
            { name: 'Sudan', code: 'SD' },
            { name: 'Suriname', code: 'SR' },
            { name: 'Svalbard and Jan Mayen', code: 'SJ' },
            { name: 'Swaziland', code: 'SZ' },
            { name: 'Sweden', code: 'SE' },
            { name: 'Switzerland', code: 'CH' },
            { name: 'Syrian Arab Republic', code: 'SY' },
            { name: 'Taiwan, Province of China', code: 'TW' },
            { name: 'Tajikistan', code: 'TJ' },
            { name: 'Tanzania, United Republic of', code: 'TZ' },
            { name: 'Thailand', code: 'TH' },
            { name: 'Timor-Leste', code: 'TL' },
            { name: 'Togo', code: 'TG' },
            { name: 'Tokelau', code: 'TK' },
            { name: 'Tonga', code: 'TO' },
            { name: 'Trinidad and Tobago', code: 'TT' },
            { name: 'Tunisia', code: 'TN' },
            { name: 'Turkey', code: 'TR' },
            { name: 'Turkmenistan', code: 'TM' },
            { name: 'Turks and Caicos Islands', code: 'TC' },
            { name: 'Tuvalu', code: 'TV' },
            { name: 'Uganda', code: 'UG' },
            { name: 'Ukraine', code: 'UA' },
            { name: 'United Arab Emirates', code: 'AE' },
            { name: 'United Kingdom', code: 'GB' },
            { name: 'United States', code: 'US' },
            { name: 'United States Minor Outlying Islands', code: 'UM' },
            { name: 'Uruguay', code: 'UY' },
            { name: 'Uzbekistan', code: 'UZ' },
            { name: 'Vanuatu', code: 'VU' },
            { name: 'Venezuela', code: 'VE' },
            { name: 'Vietnam', code: 'VN' },
            { name: 'Virgin Islands, British', code: 'VG' },
            { name: 'Virgin Islands, U.S.', code: 'VI' },
            { name: 'Wallis and Futuna', code: 'WF' },
            { name: 'Western Sahara', code: 'EH' },
            { name: 'Yemen', code: 'YE' },
            { name: 'Zambia', code: 'ZM' },
            { name: 'Zimbabwe', code: 'ZW' }
        ];
        $scope._Contact = true;
        $scope.CreateContact = () => {
            $scope._Contact = false;
        }
        $scope.hideContact = () => {
            $scope._Contact = true;
            $scope._ContactId = -1;
        }
        $scope.contactList = [];
        $scope.Create_Contact = () => {
            $scope.contactList.push($scope._contact);
            $scope._Contact = true;
            $scope._contact = {};
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
        //console.log("hello", holder);

        $scope.files = [];
        $scope.openProjectFileDialog = function () {
            $('#projectFile').click();
        }
  
        try {
            $scope.uppy = new Uppy.Core({ autoProceed: true })
                .use(Uppy.Dashboard, {
                    id: 'ringring',
                    target: '#uppy-uploader',
                    allowMultipleUploads: true,
                    //trigger: '#uppy-uploader',
                    metaFields: [],
                    //trigger: '#uppy-select-files',
                    inline: true,
                    height: 200,
                    //defaultTabIcon: defaultTabIcon,
                    showLinkToFileUploadResult: false,
                    showProgressDetails: false,
                    hideUploadButton: false,
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
                    theme: 'light'
                })
                .use(Uppy.Tus,
                    {
                        endpoint: root + 'files/',
                        resume: true,
                        retryDelays: [0, 1000, 3000, 5000],
                        chunkSize: 5242880
                    });

            $scope.uppy.on('complete', (result) => {
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
                    //fileObj.Type = 'V';
                    fileObj.DocumentCategoryId = parseInt($stateParams.docCatId);
                    fileObj.Size = file.size;
                    fileObj.ContentType = file.type;
                    var validImageTypes = ['image/gif', 'image/jpeg', 'image/png'];
                    if (validImageTypes.includes(file.type)) {
                        // invalid file type code goes here.
                        fileObj.Type = "image";
                    }
                    fileObj.Default = $scope.isItDocDacThumbnail;
                    fileObj.SceneId = null;
                    $scope.uploadedFiles.push(fileObj);
                    $scope.files.push(fileObj);
                });
                if ($scope.uploadedFiles.length > 0) {
                    $timeout(() => {
                        $scope.$apply(function () {
                            $scope.hasFilesinUppy = true;
                        });
                    });
                    $scope.uppy.reset();
                }
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
            size: 1200
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

        }
        $scope._ContactId = -1;
        $scope.save = () => {
            if ($scope.Agency.Country) {
                var Country = $scope.Agency.Country.name;
                $scope.Agency.Country = Country;
           
            }
            $scope.Agency.Is_Deleted = false;
            $scope.Agency.ProjectId = projectId;
            var agencyDto = {
                "Agency": $scope.Agency,
                "AgencyContact": $scope.contactList
            }
            console.log($scope.agencyDto);
            $http.post(root + 'api/scenesandscript/CreateAgency/', agencyDto).then(function (resp) {
                if (resp.status === 200) {
                   
                        toaster.pop({
                            type: 'success',
                            title: 'Success',
                            body: 'Agency created successfully!',
                        });
                        if ($scope.uploadedFiles) {
                            if ($scope.uploadedFiles.length > 0) {

                          

                                angular.forEach($scope.uploadedFiles, function (item) {

                                    item.AgencyId = resp.data.Id;

                                });
                                $http.post(root + 'api/DocumentFiles/PostCastDocumentFiles', $scope.uploadedFiles).then(
                                    function success(resp) {
                                        if (resp.data.length > 0) {

                                            $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount + $scope.uploadedFiles.length;
                                            //resp.data.forEach((file) => {
                                            //    $scope.files.push(file);
                                            //    if ($scope.isItDocDacThumbnail)
                                            //        $scope.defaultImage = file.FileId;
                                            //});
                                            $scope.uppy.reset();
                                        }
                                    }
                                    , function error() { });
                            }
                        


                       
                    }
                    $scope.cancel();
                    $rootScope.getAgency();
                }
            }, function error() { });
        }
        $scope.EditContact = (index) => {
            $scope._ContactId = index;
        }
        $scope.RemoveContact = (index) => {
            $scope.contactList.splice(index, 1);
        }
    });
});
myApp.controller('CreateTalentCtrl', function ($scope, $rootScope, $filter, $http, $uibModal, $uibModalInstance, $timeout, toaster, $ngConfirm, $stateParams, $state, title, projectId) {
    angular.element(document).ready(function () {
        $timeout(function () {
        $scope.title = title;
        $scope.cancel = () => {
            $uibModalInstance.dismiss();
        }

        $scope.openFileViewer = function (file, allFiles) {
            //console.log(project);
            setTimeout(function () {
                $("#fileViewer").css('z-index', '10000000');
            }, 1000);
            modalInstance = $uibModal.open({
                animation: false,

                templateUrl: root + 'js/ng-templates/centralized-templates/files-viewer.html',
                controller: 'fileViewerCtrl',
                size: 'lg',
                backdrop: 'static',
                resolve: {
                    fileObj: function () {
                        return file;
                    },
                    allFiles: function () {
                        return allFiles;
                    },
                    projectUsersAndGroups: function () {
                        return $scope.projectUsersAndGroups
                    },
                    fileProfileLink: function () {
                        return 'scenesandscripts.scene_details.fileProfile'
                    }
                }
            }).rendered.then(function () {
                $uibModalStack.getTop().value.modalDomEl.attr('id', "fileViewer");
            });
            modalInstance.result.then(function () {
            }, function (data) {
                console.log(data, 'from modal');
                //$scope.getProjects();
            });

        }
        $scope.files = [];
        $scope.talent = { FirstCountry: null, SecondCountry: null, ProdCountry: null, AgencyId: null, AgeMin: 0, AgeMax: 110 };
        $scope.ProjectId = projectId;
        $scope.getAgency = () => {
            $http.get(root + 'api/ScenesandScript/GetAgency?projectid=' + projectId).then(function (response) {
                if (response.status === 200) {
                    $scope.agency = [];
                    angular.forEach(response.data, function (item) { $scope.agency.push(item.Agency) });
                    $scope.filtered_agency = $scope.agency;
                }
            }, function (error) { });
        }
        //$scope.getAgency();
        $scope.countries = [
            { name: 'Afghanistan', code: 'AF' },
            { name: 'Åland Islands', code: 'AX' },
            { name: 'Albania', code: 'AL' },
            { name: 'Algeria', code: 'DZ' },
            { name: 'American Samoa', code: 'AS' },
            { name: 'Andorra', code: 'AD' },
            { name: 'Angola', code: 'AO' },
            { name: 'Anguilla', code: 'AI' },
            { name: 'Antarctica', code: 'AQ' },
            { name: 'Antigua and Barbuda', code: 'AG' },
            { name: 'Argentina', code: 'AR' },
            { name: 'Armenia', code: 'AM' },
            { name: 'Aruba', code: 'AW' },
            { name: 'Australia', code: 'AU' },
            { name: 'Austria', code: 'AT' },
            { name: 'Azerbaijan', code: 'AZ' },
            { name: 'Bahamas', code: 'BS' },
            { name: 'Bahrain', code: 'BH' },
            { name: 'Bangladesh', code: 'BD' },
            { name: 'Barbados', code: 'BB' },
            { name: 'Belarus', code: 'BY' },
            { name: 'Belgium', code: 'BE' },
            { name: 'Belize', code: 'BZ' },
            { name: 'Benin', code: 'BJ' },
            { name: 'Bermuda', code: 'BM' },
            { name: 'Bhutan', code: 'BT' },
            { name: 'Bolivia', code: 'BO' },
            { name: 'Bosnia and Herzegovina', code: 'BA' },
            { name: 'Botswana', code: 'BW' },
            { name: 'Bouvet Island', code: 'BV' },
            { name: 'Brazil', code: 'BR' },
            { name: 'British Indian Ocean Territory', code: 'IO' },
            { name: 'Brunei Darussalam', code: 'BN' },
            { name: 'Bulgaria', code: 'BG' },
            { name: 'Burkina Faso', code: 'BF' },
            { name: 'Burundi', code: 'BI' },
            { name: 'Cambodia', code: 'KH' },
            { name: 'Cameroon', code: 'CM' },
            { name: 'Canada', code: 'CA' },
            { name: 'Cape Verde', code: 'CV' },
            { name: 'Cayman Islands', code: 'KY' },
            { name: 'Central African Republic', code: 'CF' },
            { name: 'Chad', code: 'TD' },
            { name: 'Chile', code: 'CL' },
            { name: 'China', code: 'CN' },
            { name: 'Christmas Island', code: 'CX' },
            { name: 'Cocos (Keeling) Islands', code: 'CC' },
            { name: 'Colombia', code: 'CO' },
            { name: 'Comoros', code: 'KM' },
            { name: 'Congo', code: 'CG' },
            { name: 'Congo, The Democratic Republic of the', code: 'CD' },
            { name: 'Cook Islands', code: 'CK' },
            { name: 'Costa Rica', code: 'CR' },
            { name: 'Cote D\'Ivoire', code: 'CI' },
            { name: 'Croatia', code: 'HR' },
            { name: 'Cuba', code: 'CU' },
            { name: 'Cyprus', code: 'CY' },
            { name: 'Czech Republic', code: 'CZ' },
            { name: 'Denmark', code: 'DK' },
            { name: 'Djibouti', code: 'DJ' },
            { name: 'Dominica', code: 'DM' },
            { name: 'Dominican Republic', code: 'DO' },
            { name: 'Ecuador', code: 'EC' },
            { name: 'Egypt', code: 'EG' },
            { name: 'El Salvador', code: 'SV' },
            { name: 'Equatorial Guinea', code: 'GQ' },
            { name: 'Eritrea', code: 'ER' },
            { name: 'Estonia', code: 'EE' },
            { name: 'Ethiopia', code: 'ET' },
            { name: 'Falkland Islands (Malvinas)', code: 'FK' },
            { name: 'Faroe Islands', code: 'FO' },
            { name: 'Fiji', code: 'FJ' },
            { name: 'Finland', code: 'FI' },
            { name: 'France', code: 'FR' },
            { name: 'French Guiana', code: 'GF' },
            { name: 'French Polynesia', code: 'PF' },
            { name: 'French Southern Territories', code: 'TF' },
            { name: 'Gabon', code: 'GA' },
            { name: 'Gambia', code: 'GM' },
            { name: 'Georgia', code: 'GE' },
            { name: 'Germany', code: 'DE' },
            { name: 'Ghana', code: 'GH' },
            { name: 'Gibraltar', code: 'GI' },
            { name: 'Greece', code: 'GR' },
            { name: 'Greenland', code: 'GL' },
            { name: 'Grenada', code: 'GD' },
            { name: 'Guadeloupe', code: 'GP' },
            { name: 'Guam', code: 'GU' },
            { name: 'Guatemala', code: 'GT' },
            { name: 'Guernsey', code: 'GG' },
            { name: 'Guinea', code: 'GN' },
            { name: 'Guinea-Bissau', code: 'GW' },
            { name: 'Guyana', code: 'GY' },
            { name: 'Haiti', code: 'HT' },
            { name: 'Heard Island and Mcdonald Islands', code: 'HM' },
            { name: 'Holy See (Vatican City State)', code: 'VA' },
            { name: 'Honduras', code: 'HN' },
            { name: 'Hong Kong', code: 'HK' },
            { name: 'Hungary', code: 'HU' },
            { name: 'Iceland', code: 'IS' },
            { name: 'India', code: 'IN' },
            { name: 'Indonesia', code: 'ID' },
            { name: 'Iran, Islamic Republic Of', code: 'IR' },
            { name: 'Iraq', code: 'IQ' },
            { name: 'Ireland', code: 'IE' },
            { name: 'Isle of Man', code: 'IM' },
            { name: 'Israel', code: 'IL' },
            { name: 'Italy', code: 'IT' },
            { name: 'Jamaica', code: 'JM' },
            { name: 'Japan', code: 'JP' },
            { name: 'Jersey', code: 'JE' },
            { name: 'Jordan', code: 'JO' },
            { name: 'Kazakhstan', code: 'KZ' },
            { name: 'Kenya', code: 'KE' },
            { name: 'Kiribati', code: 'KI' },
            { name: 'Korea, Democratic People\'s Republic of', code: 'KP' },
            { name: 'Korea, Republic of', code: 'KR' },
            { name: 'Kuwait', code: 'KW' },
            { name: 'Kyrgyzstan', code: 'KG' },
            { name: 'Lao People\'s Democratic Republic', code: 'LA' },
            { name: 'Latvia', code: 'LV' },
            { name: 'Lebanon', code: 'LB' },
            { name: 'Lesotho', code: 'LS' },
            { name: 'Liberia', code: 'LR' },
            { name: 'Libyan Arab Jamahiriya', code: 'LY' },
            { name: 'Liechtenstein', code: 'LI' },
            { name: 'Lithuania', code: 'LT' },
            { name: 'Luxembourg', code: 'LU' },
            { name: 'Macao', code: 'MO' },
            { name: 'Macedonia, The Former Yugoslav Republic of', code: 'MK' },
            { name: 'Madagascar', code: 'MG' },
            { name: 'Malawi', code: 'MW' },
            { name: 'Malaysia', code: 'MY' },
            { name: 'Maldives', code: 'MV' },
            { name: 'Mali', code: 'ML' },
            { name: 'Malta', code: 'MT' },
            { name: 'Marshall Islands', code: 'MH' },
            { name: 'Martinique', code: 'MQ' },
            { name: 'Mauritania', code: 'MR' },
            { name: 'Mauritius', code: 'MU' },
            { name: 'Mayotte', code: 'YT' },
            { name: 'Mexico', code: 'MX' },
            { name: 'Micronesia, Federated States of', code: 'FM' },
            { name: 'Moldova, Republic of', code: 'MD' },
            { name: 'Monaco', code: 'MC' },
            { name: 'Mongolia', code: 'MN' },
            { name: 'Montserrat', code: 'MS' },
            { name: 'Morocco', code: 'MA' },
            { name: 'Mozambique', code: 'MZ' },
            { name: 'Myanmar', code: 'MM' },
            { name: 'Namibia', code: 'NA' },
            { name: 'Nauru', code: 'NR' },
            { name: 'Nepal', code: 'NP' },
            { name: 'Netherlands', code: 'NL' },
            { name: 'Netherlands Antilles', code: 'AN' },
            { name: 'New Caledonia', code: 'NC' },
            { name: 'New Zealand', code: 'NZ' },
            { name: 'Nicaragua', code: 'NI' },
            { name: 'Niger', code: 'NE' },
            { name: 'Nigeria', code: 'NG' },
            { name: 'Niue', code: 'NU' },
            { name: 'Norfolk Island', code: 'NF' },
            { name: 'Northern Mariana Islands', code: 'MP' },
            { name: 'Norway', code: 'NO' },
            { name: 'Oman', code: 'OM' },
            { name: 'Pakistan', code: 'PK' },
            { name: 'Palau', code: 'PW' },
            { name: 'Palestinian Territory, Occupied', code: 'PS' },
            { name: 'Panama', code: 'PA' },
            { name: 'Papua New Guinea', code: 'PG' },
            { name: 'Paraguay', code: 'PY' },
            { name: 'Peru', code: 'PE' },
            { name: 'Philippines', code: 'PH' },
            { name: 'Pitcairn', code: 'PN' },
            { name: 'Poland', code: 'PL' },
            { name: 'Portugal', code: 'PT' },
            { name: 'Puerto Rico', code: 'PR' },
            { name: 'Qatar', code: 'QA' },
            { name: 'Reunion', code: 'RE' },
            { name: 'Romania', code: 'RO' },
            { name: 'Russian Federation', code: 'RU' },
            { name: 'Rwanda', code: 'RW' },
            { name: 'Saint Helena', code: 'SH' },
            { name: 'Saint Kitts and Nevis', code: 'KN' },
            { name: 'Saint Lucia', code: 'LC' },
            { name: 'Saint Pierre and Miquelon', code: 'PM' },
            { name: 'Saint Vincent and the Grenadines', code: 'VC' },
            { name: 'Samoa', code: 'WS' },
            { name: 'San Marino', code: 'SM' },
            { name: 'Sao Tome and Principe', code: 'ST' },
            { name: 'Saudi Arabia', code: 'SA' },
            { name: 'Senegal', code: 'SN' },
            { name: 'Serbia and Montenegro', code: 'CS' },
            { name: 'Seychelles', code: 'SC' },
            { name: 'Sierra Leone', code: 'SL' },
            { name: 'Singapore', code: 'SG' },
            { name: 'Slovakia', code: 'SK' },
            { name: 'Slovenia', code: 'SI' },
            { name: 'Solomon Islands', code: 'SB' },
            { name: 'Somalia', code: 'SO' },
            { name: 'South Africa', code: 'ZA' },
            { name: 'South Georgia and the South Sandwich Islands', code: 'GS' },
            { name: 'Spain', code: 'ES' },
            { name: 'Sri Lanka', code: 'LK' },
            { name: 'Sudan', code: 'SD' },
            { name: 'Suriname', code: 'SR' },
            { name: 'Svalbard and Jan Mayen', code: 'SJ' },
            { name: 'Swaziland', code: 'SZ' },
            { name: 'Sweden', code: 'SE' },
            { name: 'Switzerland', code: 'CH' },
            { name: 'Syrian Arab Republic', code: 'SY' },
            { name: 'Taiwan, Province of China', code: 'TW' },
            { name: 'Tajikistan', code: 'TJ' },
            { name: 'Tanzania, United Republic of', code: 'TZ' },
            { name: 'Thailand', code: 'TH' },
            { name: 'Timor-Leste', code: 'TL' },
            { name: 'Togo', code: 'TG' },
            { name: 'Tokelau', code: 'TK' },
            { name: 'Tonga', code: 'TO' },
            { name: 'Trinidad and Tobago', code: 'TT' },
            { name: 'Tunisia', code: 'TN' },
            { name: 'Turkey', code: 'TR' },
            { name: 'Turkmenistan', code: 'TM' },
            { name: 'Turks and Caicos Islands', code: 'TC' },
            { name: 'Tuvalu', code: 'TV' },
            { name: 'Uganda', code: 'UG' },
            { name: 'Ukraine', code: 'UA' },
            { name: 'United Arab Emirates', code: 'AE' },
            { name: 'United Kingdom', code: 'GB' },
            { name: 'United States', code: 'US' },
            { name: 'United States Minor Outlying Islands', code: 'UM' },
            { name: 'Uruguay', code: 'UY' },
            { name: 'Uzbekistan', code: 'UZ' },
            { name: 'Vanuatu', code: 'VU' },
            { name: 'Venezuela', code: 'VE' },
            { name: 'Vietnam', code: 'VN' },
            { name: 'Virgin Islands, British', code: 'VG' },
            { name: 'Virgin Islands, U.S.', code: 'VI' },
            { name: 'Wallis and Futuna', code: 'WF' },
            { name: 'Western Sahara', code: 'EH' },
            { name: 'Yemen', code: 'YE' },
            { name: 'Zambia', code: 'ZM' },
            { name: 'Zimbabwe', code: 'ZW' }
        ];
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
        //console.log("hello", holder);

        $scope.files = [];
        $scope.openProjectFileDialog = function () {
            $('#projectFile').click();
        }

        try {
            $scope.uppy = new Uppy.Core({ autoProceed: true })
                .use(Uppy.Dashboard, {
                    id: 'ringring',
                    target: '#uppy-uploader',
                    allowMultipleUploads: true,
                    //trigger: '#uppy-uploader',
                    metaFields: [],
                    //trigger: '#uppy-select-files',
                    inline: true,
                    height: 200,
                    //defaultTabIcon: defaultTabIcon,
                    showLinkToFileUploadResult: false,
                    showProgressDetails: false,
                    hideUploadButton: false,
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
                    theme: 'light'
                })
                .use(Uppy.Tus,
                    {
                        endpoint: root + 'files/',
                        resume: true,
                        retryDelays: [0, 1000, 3000, 5000],
                        chunkSize: 5242880
                    });

            $scope.uppy.on('complete', (result) => {
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
                    //fileObj.Type = 'V';
                    fileObj.DocumentCategoryId = parseInt($stateParams.docCatId);
                    fileObj.Size = file.size;
                    fileObj.ContentType = file.type;
                    var validImageTypes = ['image/gif', 'image/jpeg', 'image/png'];
                    if (validImageTypes.includes(file.type)) {
                        // invalid file type code goes here.
                        fileObj.Type = "image";
                    }
                    fileObj.Default = $scope.isItDocDacThumbnail;
                    fileObj.SceneId = null;
                    $scope.uploadedFiles.push(fileObj);
                    $scope.files.push(fileObj);
                });
                if ($scope.uploadedFiles.length > 0) {
                    $timeout(() => {
                        $scope.$apply(function () {
                            $scope.hasFilesinUppy = true;
                        });
                    });
                    $scope.uppy.reset();
                }
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
            size: 1200
        }
        $scope.deleteFile = function ($event, file) {
            //$event.stopPropagation();
            var index = $scope.files.indexOf(file);
            if (index > -1)
                $scope.files.splice(index, 1);

            $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount - 1;
            if ($scope.tabContentLength.filesCount < 0) {
                $scope.tabContentLength.filesCount = 0;
            }

        }
        $scope.agencyBtn = false;
        $scope.CreatAgencyBtn = true;
        $scope.showAgency = () => {
            $scope.agencyBtn = true;
        }
        $scope.hideAgency = () => {
            $scope.agencyBtn = false;
        }
        $scope.complete_Agency = function (e, string) {
            if (string == null || string == "undefined") {

                $scope.CreatAgencyBtn = true;
                $scope.filtered_agency = $scope.agency;
            } else if (e.keyCode === 13) {
                $scope.create_agency(string);
            } else {

                var output = [];
                angular.forEach($scope.agency, function (item) {

                    if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                        output.push(item);
                        //var exist = $scope.EditScene.CharacterId.includes(item.Id.toString());

                        //if (exist) {
                        //    chk = true;

                        //}
                    }
                });
                if (output.length > 0) {
                    $scope.CreatAgencyBtn = true;
                    $scope.filtered_agency = output;
                }
                if (output.length == 0) {
                    $scope.filtered_agency = output;
                    $scope.CreatAgencyBtn = false;
                }


            }

        }
        $scope.create_agency = (string) => {
            var Agency = { Name: string, ProjectId: parseInt(projectId) };
            var agencyDto = {
                "Agency": Agency,
                "AgencyContact": []
            }
            console.log($scope.agencyDto);
            $http.post(root + 'api/scenesandscript/CreateAgency/', agencyDto).then(function (resp) {
                if (resp.status === 200) {
                    $scope.CreatAgencyBtn = true;
                    $rootScope.getAgency();
                    $scope.getAgency();
                }
            }, function error() { });
        }
        $scope.add_agency = (agency) => {
            $scope.talent.AgencyId = agency;
            $scope._agency = agency.Name;
            $scope.agencyBtn = false;
        }
        $scope.save = () => {
            if ($scope.talent.AgencyId) {
                $scope.talent.AgencyId = $scope.talent.AgencyId.Id;
            }
            if ($scope.talent.FirstCountry) {
                var Country = $scope.talent.FirstCountry.name
                $scope.talent.FirstCountry = Country;

            } if ($scope.talent.SecondCountry) {
                var sCountry = $scope.talent.SecondCountry.name
                $scope.talent.SecondCountry = sCountry;

            } if ($scope.talent.ProdCountry) {
                var pCountry = $scope.talent.ProdCountry.name
                $scope.talent.ProdCountry = pCountry;

            }
            $scope.talent.Is_Deleted = false;
            $scope.talent.ProjectId = projectId;


            $http.post(root + 'api/scenesandscript/CreateTalent/', $scope.talent).then(function (resp) {
                if (resp.status === 200) {

                    toaster.pop({
                        type: 'success',
                        title: 'Success',
                        body: 'Talent created successfully!',
                    });
                    if ($scope.uploadedFiles) {
                        if ($scope.uploadedFiles.length > 0) {



                            angular.forEach($scope.uploadedFiles, function (item) {

                                item.TalentId = resp.data.Id;

                            });

                            $http.post(root + 'api/DocumentFiles/PostCastDocumentFiles', $scope.uploadedFiles).then(
                                function success(resp) {
                                    if (resp.data.length > 0) {

                                        $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount + $scope.uploadedFiles.length;
                                        //resp.data.forEach((file) => {
                                        //    $scope.files.push(file);
                                        //    if ($scope.isItDocDacThumbnail)
                                        //        $scope.defaultImage = file.FileId;
                                        //});
                                        $scope.uppy.reset();
                                    }
                                }
                                , function error() { });
                        }




                    }
                    $scope.cancel();
                    $rootScope.getTalent();
                }
            }, function error() { });
        }
        }, 100)
    });
});

myApp.controller('CreateActorCtrl', function ($scope, $rootScope, $filter, $http, $uibModal, $uibModalInstance, $timeout, toaster, $ngConfirm, $stateParams, $state, title, projectId) {
    angular.element(document).ready(function () {
        $timeout(function () {
            $scope.title = title;
            $scope.cancel = () => {
                $uibModalInstance.dismiss();
            }
            //Create-Agency
            var modalInstance = null;
            $scope.create_agency = function (ch) {
                modalInstance = $uibModal.open({
                    animation: false,
                    templateUrl: root + 'js/ng-templates/Cast/create-agency.html',
                    controller: 'CreateAgencyCtrl',
                    size: 'md',
                    backdrop: 'static',
                    resolve: {
                        title: function () {
                            return 'Create Agency ';
                        },
                        projectItem: function () {
                            return null;
                        },
                        projectId: function () {
                            return projectId;
                        }, Name: function () {
                            return ch;
                        }

                    }
                });
                modalInstance.result.then(function () {
                    $scope.getAgency();
                }, function (data) {
                    $scope.getAgency();
                });
            }
            $scope.openFileViewer = function (file, allFiles) {
                //console.log(project);
                setTimeout(function () {
                    $("#fileViewer").css('z-index', '10000000');
                }, 1000);
                modalInstance = $uibModal.open({
                    animation: false,
                    templateUrl: root + 'js/ng-templates/centralized-templates/files-viewer.html',
                    controller: 'fileViewerCtrl',
                    size: 'lg',
                    backdrop: 'static',
                    resolve: {
                        fileObj: function () {
                            return file;
                        },
                        allFiles: function () {
                            return allFiles;
                        },
                        projectUsersAndGroups: function () {
                            return $scope.projectUsersAndGroups
                        },
                        fileProfileLink: function () {
                            return 'scenesandscripts.scene_details.fileProfile'
                        }
                    }
                }).rendered.then(function () {
                    $uibModalStack.getTop().value.modalDomEl.attr('id', "fileViewer");
                });
                modalInstance.result.then(function () {
                }, function (data) {
                    console.log(data, 'from modal');
                    //$scope.getProjects();
                });

            }
            $scope.files = [];
            $scope.actor = { FirstCountry: null, SecondCountry: null, ProdCountry: null, AgencyId: null, AgeMin: 0, AgeMax: 110 };
            $scope.ProjectId = projectId;
            $scope.getAgency = () => {
                $http.get(root + 'api/ScenesandScript/GetAgency?projectid=' + projectId).then(function (response) {
                    if (response.status === 200) {
                        $scope.agency = [];
                        angular.forEach(response.data, function (item) { $scope.agency.push(item.Agency) });
                        $scope.filtered_agency = $scope.agency;
                    }
                }, function (error) { });
            }
            //$scope.getAgency();
            $scope.countries = [
                { name: 'Afghanistan', code: 'AF' },
                { name: 'Åland Islands', code: 'AX' },
                { name: 'Albania', code: 'AL' },
                { name: 'Algeria', code: 'DZ' },
                { name: 'American Samoa', code: 'AS' },
                { name: 'Andorra', code: 'AD' },
                { name: 'Angola', code: 'AO' },
                { name: 'Anguilla', code: 'AI' },
                { name: 'Antarctica', code: 'AQ' },
                { name: 'Antigua and Barbuda', code: 'AG' },
                { name: 'Argentina', code: 'AR' },
                { name: 'Armenia', code: 'AM' },
                { name: 'Aruba', code: 'AW' },
                { name: 'Australia', code: 'AU' },
                { name: 'Austria', code: 'AT' },
                { name: 'Azerbaijan', code: 'AZ' },
                { name: 'Bahamas', code: 'BS' },
                { name: 'Bahrain', code: 'BH' },
                { name: 'Bangladesh', code: 'BD' },
                { name: 'Barbados', code: 'BB' },
                { name: 'Belarus', code: 'BY' },
                { name: 'Belgium', code: 'BE' },
                { name: 'Belize', code: 'BZ' },
                { name: 'Benin', code: 'BJ' },
                { name: 'Bermuda', code: 'BM' },
                { name: 'Bhutan', code: 'BT' },
                { name: 'Bolivia', code: 'BO' },
                { name: 'Bosnia and Herzegovina', code: 'BA' },
                { name: 'Botswana', code: 'BW' },
                { name: 'Bouvet Island', code: 'BV' },
                { name: 'Brazil', code: 'BR' },
                { name: 'British Indian Ocean Territory', code: 'IO' },
                { name: 'Brunei Darussalam', code: 'BN' },
                { name: 'Bulgaria', code: 'BG' },
                { name: 'Burkina Faso', code: 'BF' },
                { name: 'Burundi', code: 'BI' },
                { name: 'Cambodia', code: 'KH' },
                { name: 'Cameroon', code: 'CM' },
                { name: 'Canada', code: 'CA' },
                { name: 'Cape Verde', code: 'CV' },
                { name: 'Cayman Islands', code: 'KY' },
                { name: 'Central African Republic', code: 'CF' },
                { name: 'Chad', code: 'TD' },
                { name: 'Chile', code: 'CL' },
                { name: 'China', code: 'CN' },
                { name: 'Christmas Island', code: 'CX' },
                { name: 'Cocos (Keeling) Islands', code: 'CC' },
                { name: 'Colombia', code: 'CO' },
                { name: 'Comoros', code: 'KM' },
                { name: 'Congo', code: 'CG' },
                { name: 'Congo, The Democratic Republic of the', code: 'CD' },
                { name: 'Cook Islands', code: 'CK' },
                { name: 'Costa Rica', code: 'CR' },
                { name: 'Cote D\'Ivoire', code: 'CI' },
                { name: 'Croatia', code: 'HR' },
                { name: 'Cuba', code: 'CU' },
                { name: 'Cyprus', code: 'CY' },
                { name: 'Czech Republic', code: 'CZ' },
                { name: 'Denmark', code: 'DK' },
                { name: 'Djibouti', code: 'DJ' },
                { name: 'Dominica', code: 'DM' },
                { name: 'Dominican Republic', code: 'DO' },
                { name: 'Ecuador', code: 'EC' },
                { name: 'Egypt', code: 'EG' },
                { name: 'El Salvador', code: 'SV' },
                { name: 'Equatorial Guinea', code: 'GQ' },
                { name: 'Eritrea', code: 'ER' },
                { name: 'Estonia', code: 'EE' },
                { name: 'Ethiopia', code: 'ET' },
                { name: 'Falkland Islands (Malvinas)', code: 'FK' },
                { name: 'Faroe Islands', code: 'FO' },
                { name: 'Fiji', code: 'FJ' },
                { name: 'Finland', code: 'FI' },
                { name: 'France', code: 'FR' },
                { name: 'French Guiana', code: 'GF' },
                { name: 'French Polynesia', code: 'PF' },
                { name: 'French Southern Territories', code: 'TF' },
                { name: 'Gabon', code: 'GA' },
                { name: 'Gambia', code: 'GM' },
                { name: 'Georgia', code: 'GE' },
                { name: 'Germany', code: 'DE' },
                { name: 'Ghana', code: 'GH' },
                { name: 'Gibraltar', code: 'GI' },
                { name: 'Greece', code: 'GR' },
                { name: 'Greenland', code: 'GL' },
                { name: 'Grenada', code: 'GD' },
                { name: 'Guadeloupe', code: 'GP' },
                { name: 'Guam', code: 'GU' },
                { name: 'Guatemala', code: 'GT' },
                { name: 'Guernsey', code: 'GG' },
                { name: 'Guinea', code: 'GN' },
                { name: 'Guinea-Bissau', code: 'GW' },
                { name: 'Guyana', code: 'GY' },
                { name: 'Haiti', code: 'HT' },
                { name: 'Heard Island and Mcdonald Islands', code: 'HM' },
                { name: 'Holy See (Vatican City State)', code: 'VA' },
                { name: 'Honduras', code: 'HN' },
                { name: 'Hong Kong', code: 'HK' },
                { name: 'Hungary', code: 'HU' },
                { name: 'Iceland', code: 'IS' },
                { name: 'India', code: 'IN' },
                { name: 'Indonesia', code: 'ID' },
                { name: 'Iran, Islamic Republic Of', code: 'IR' },
                { name: 'Iraq', code: 'IQ' },
                { name: 'Ireland', code: 'IE' },
                { name: 'Isle of Man', code: 'IM' },
                { name: 'Israel', code: 'IL' },
                { name: 'Italy', code: 'IT' },
                { name: 'Jamaica', code: 'JM' },
                { name: 'Japan', code: 'JP' },
                { name: 'Jersey', code: 'JE' },
                { name: 'Jordan', code: 'JO' },
                { name: 'Kazakhstan', code: 'KZ' },
                { name: 'Kenya', code: 'KE' },
                { name: 'Kiribati', code: 'KI' },
                { name: 'Korea, Democratic People\'s Republic of', code: 'KP' },
                { name: 'Korea, Republic of', code: 'KR' },
                { name: 'Kuwait', code: 'KW' },
                { name: 'Kyrgyzstan', code: 'KG' },
                { name: 'Lao People\'s Democratic Republic', code: 'LA' },
                { name: 'Latvia', code: 'LV' },
                { name: 'Lebanon', code: 'LB' },
                { name: 'Lesotho', code: 'LS' },
                { name: 'Liberia', code: 'LR' },
                { name: 'Libyan Arab Jamahiriya', code: 'LY' },
                { name: 'Liechtenstein', code: 'LI' },
                { name: 'Lithuania', code: 'LT' },
                { name: 'Luxembourg', code: 'LU' },
                { name: 'Macao', code: 'MO' },
                { name: 'Macedonia, The Former Yugoslav Republic of', code: 'MK' },
                { name: 'Madagascar', code: 'MG' },
                { name: 'Malawi', code: 'MW' },
                { name: 'Malaysia', code: 'MY' },
                { name: 'Maldives', code: 'MV' },
                { name: 'Mali', code: 'ML' },
                { name: 'Malta', code: 'MT' },
                { name: 'Marshall Islands', code: 'MH' },
                { name: 'Martinique', code: 'MQ' },
                { name: 'Mauritania', code: 'MR' },
                { name: 'Mauritius', code: 'MU' },
                { name: 'Mayotte', code: 'YT' },
                { name: 'Mexico', code: 'MX' },
                { name: 'Micronesia, Federated States of', code: 'FM' },
                { name: 'Moldova, Republic of', code: 'MD' },
                { name: 'Monaco', code: 'MC' },
                { name: 'Mongolia', code: 'MN' },
                { name: 'Montserrat', code: 'MS' },
                { name: 'Morocco', code: 'MA' },
                { name: 'Mozambique', code: 'MZ' },
                { name: 'Myanmar', code: 'MM' },
                { name: 'Namibia', code: 'NA' },
                { name: 'Nauru', code: 'NR' },
                { name: 'Nepal', code: 'NP' },
                { name: 'Netherlands', code: 'NL' },
                { name: 'Netherlands Antilles', code: 'AN' },
                { name: 'New Caledonia', code: 'NC' },
                { name: 'New Zealand', code: 'NZ' },
                { name: 'Nicaragua', code: 'NI' },
                { name: 'Niger', code: 'NE' },
                { name: 'Nigeria', code: 'NG' },
                { name: 'Niue', code: 'NU' },
                { name: 'Norfolk Island', code: 'NF' },
                { name: 'Northern Mariana Islands', code: 'MP' },
                { name: 'Norway', code: 'NO' },
                { name: 'Oman', code: 'OM' },
                { name: 'Pakistan', code: 'PK' },
                { name: 'Palau', code: 'PW' },
                { name: 'Palestinian Territory, Occupied', code: 'PS' },
                { name: 'Panama', code: 'PA' },
                { name: 'Papua New Guinea', code: 'PG' },
                { name: 'Paraguay', code: 'PY' },
                { name: 'Peru', code: 'PE' },
                { name: 'Philippines', code: 'PH' },
                { name: 'Pitcairn', code: 'PN' },
                { name: 'Poland', code: 'PL' },
                { name: 'Portugal', code: 'PT' },
                { name: 'Puerto Rico', code: 'PR' },
                { name: 'Qatar', code: 'QA' },
                { name: 'Reunion', code: 'RE' },
                { name: 'Romania', code: 'RO' },
                { name: 'Russian Federation', code: 'RU' },
                { name: 'Rwanda', code: 'RW' },
                { name: 'Saint Helena', code: 'SH' },
                { name: 'Saint Kitts and Nevis', code: 'KN' },
                { name: 'Saint Lucia', code: 'LC' },
                { name: 'Saint Pierre and Miquelon', code: 'PM' },
                { name: 'Saint Vincent and the Grenadines', code: 'VC' },
                { name: 'Samoa', code: 'WS' },
                { name: 'San Marino', code: 'SM' },
                { name: 'Sao Tome and Principe', code: 'ST' },
                { name: 'Saudi Arabia', code: 'SA' },
                { name: 'Senegal', code: 'SN' },
                { name: 'Serbia and Montenegro', code: 'CS' },
                { name: 'Seychelles', code: 'SC' },
                { name: 'Sierra Leone', code: 'SL' },
                { name: 'Singapore', code: 'SG' },
                { name: 'Slovakia', code: 'SK' },
                { name: 'Slovenia', code: 'SI' },
                { name: 'Solomon Islands', code: 'SB' },
                { name: 'Somalia', code: 'SO' },
                { name: 'South Africa', code: 'ZA' },
                { name: 'South Georgia and the South Sandwich Islands', code: 'GS' },
                { name: 'Spain', code: 'ES' },
                { name: 'Sri Lanka', code: 'LK' },
                { name: 'Sudan', code: 'SD' },
                { name: 'Suriname', code: 'SR' },
                { name: 'Svalbard and Jan Mayen', code: 'SJ' },
                { name: 'Swaziland', code: 'SZ' },
                { name: 'Sweden', code: 'SE' },
                { name: 'Switzerland', code: 'CH' },
                { name: 'Syrian Arab Republic', code: 'SY' },
                { name: 'Taiwan, Province of China', code: 'TW' },
                { name: 'Tajikistan', code: 'TJ' },
                { name: 'Tanzania, United Republic of', code: 'TZ' },
                { name: 'Thailand', code: 'TH' },
                { name: 'Timor-Leste', code: 'TL' },
                { name: 'Togo', code: 'TG' },
                { name: 'Tokelau', code: 'TK' },
                { name: 'Tonga', code: 'TO' },
                { name: 'Trinidad and Tobago', code: 'TT' },
                { name: 'Tunisia', code: 'TN' },
                { name: 'Turkey', code: 'TR' },
                { name: 'Turkmenistan', code: 'TM' },
                { name: 'Turks and Caicos Islands', code: 'TC' },
                { name: 'Tuvalu', code: 'TV' },
                { name: 'Uganda', code: 'UG' },
                { name: 'Ukraine', code: 'UA' },
                { name: 'United Arab Emirates', code: 'AE' },
                { name: 'United Kingdom', code: 'GB' },
                { name: 'United States', code: 'US' },
                { name: 'United States Minor Outlying Islands', code: 'UM' },
                { name: 'Uruguay', code: 'UY' },
                { name: 'Uzbekistan', code: 'UZ' },
                { name: 'Vanuatu', code: 'VU' },
                { name: 'Venezuela', code: 'VE' },
                { name: 'Vietnam', code: 'VN' },
                { name: 'Virgin Islands, British', code: 'VG' },
                { name: 'Virgin Islands, U.S.', code: 'VI' },
                { name: 'Wallis and Futuna', code: 'WF' },
                { name: 'Western Sahara', code: 'EH' },
                { name: 'Yemen', code: 'YE' },
                { name: 'Zambia', code: 'ZM' },
                { name: 'Zimbabwe', code: 'ZW' }
            ];
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
            //console.log("hello", holder);

            $scope.files = [];
            $scope.openProjectFileDialog = function () {
                $('#projectFile').click();
            }

            try {
                $scope.uppy = new Uppy.Core({ autoProceed: true })
                    .use(Uppy.Dashboard, {
                        id: 'ringring',
                        target: '#uppy-uploader',
                        allowMultipleUploads: true,
                        //trigger: '#uppy-uploader',
                        metaFields: [],
                        //trigger: '#uppy-select-files',
                        inline: true,
                        height: 200,
                        //defaultTabIcon: defaultTabIcon,
                        showLinkToFileUploadResult: false,
                        showProgressDetails: false,
                        hideUploadButton: false,
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
                        theme: 'light'
                    })
                    .use(Uppy.Tus,
                        {
                            endpoint: root + 'files/',
                            resume: true,
                            retryDelays: [0, 1000, 3000, 5000],
                            chunkSize: 5242880
                        });

                $scope.uppy.on('complete', (result) => {
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
                        //fileObj.Type = 'V';
                        fileObj.DocumentCategoryId = parseInt($stateParams.docCatId);
                        fileObj.Size = file.size;
                        fileObj.ContentType = file.type;
                        var validImageTypes = ['image/gif', 'image/jpeg', 'image/png'];
                        if (validImageTypes.includes(file.type)) {
                            // invalid file type code goes here.
                            fileObj.Type = "image";
                        }
                        fileObj.Default = $scope.isItDocDacThumbnail;
                        fileObj.SceneId = null;
                        $scope.uploadedFiles.push(fileObj);
                        $scope.files.push(fileObj);
                    });
                    if ($scope.uploadedFiles.length > 0) {
                        $timeout(() => {
                            $scope.$apply(function () {
                                $scope.hasFilesinUppy = true;
                            });
                        });
                        $scope.uppy.reset();
                    }
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
                size: 1200
            }
            $scope.deleteFile = function ($event, file) {
                //$event.stopPropagation();
                var index = $scope.files.indexOf(file);
                if (index > -1)
                    $scope.files.splice(index, 1);

                $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount - 1;
                if ($scope.tabContentLength.filesCount < 0) {
                    $scope.tabContentLength.filesCount = 0;
                }

            }
            $scope.agencyBtn = false;
            $scope.CreatAgencyBtn = true;
            $scope.showAgency = () => {
                $scope.agencyBtn = true;
            }
            $scope.hideAgency = () => {
                $scope.agencyBtn = false;
            }
            $scope.complete_Agency = function (e, string) {
                if (string == null || string == "undefined") {

                    $scope.CreatAgencyBtn = true;
                    $scope.filtered_agency = $scope.agency;
                } else if (e.keyCode === 13) {
                    $scope.create_agency(string);
                } else {

                    var output = [];
                    angular.forEach($scope.agency, function (item) {

                        if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                            output.push(item);
                            //var exist = $scope.EditScene.CharacterId.includes(item.Id.toString());

                            //if (exist) {
                            //    chk = true;

                            //}
                        }
                    });
                    if (output.length > 0) {
                        $scope.CreatAgencyBtn = true;
                        $scope.filtered_agency = output;
                    }
                    if (output.length == 0) {
                        $scope.filtered_agency = output;
                        $scope.CreatAgencyBtn = false;
                    }


                }

            }
            $scope.create_agency = (string) => {
                var Agency = { Name: string, ProjectId: parseInt(projectId) };
                var agencyDto = {
                    "Agency": Agency,
                    "AgencyContact": []
                }
                console.log($scope.agencyDto);
                $http.post(root + 'api/scenesandscript/CreateAgency/', agencyDto).then(function (resp) {
                    if (resp.status === 200) {
                        $scope.CreatAgencyBtn = true;
                        $rootScope.getAgency();
                        $scope.getAgency();
                    }
                }, function error() { });
            }
            $scope.add_agency = (agency) => {
                $scope.actor.AgencyId = agency;
                $scope._agency = agency.Name;
                $scope.agencyBtn = false;
            }

            $scope.save = () => {
                if ($scope.actor.AgencyId) {
                    $scope.actor.AgencyId = $scope.actor.AgencyId.Id;
                }
                if ($scope.actor.FirstCountry) {
                    var Country = $scope.actor.FirstCountry.name
                    $scope.actor.FirstCountry = Country;

                } if ($scope.actor.SecondCountry) {
                    var sCountry = $scope.actor.SecondCountry.name
                    $scope.actor.SecondCountry = sCountry;

                } if ($scope.actor.ProdCountry) {
                    var pCountry = $scope.actor.ProdCountry.name
                    $scope.actor.ProdCountry = pCountry;

                }
                $scope.actor.Is_Deleted = false;
                $scope.actor.ProjectId = projectId;

                console.log($scope.agencyDto);
                $http.post(root + 'api/scenesandscript/CreateActor/', $scope.actor).then(function (resp) {
                    if (resp.status === 200) {

                        toaster.pop({
                            type: 'success',
                            title: 'Success',
                            body: 'Actor created successfully!',
                        });
                        if ($scope.uploadedFiles) {
                            if ($scope.uploadedFiles.length > 0) {



                                angular.forEach($scope.uploadedFiles, function (item) {

                                    item.ActorId = resp.data.Id;

                                });
                                $http.post(root + 'api/DocumentFiles/PostCastDocumentFiles', $scope.uploadedFiles).then(
                                    function success(resp) {
                                        if (resp.data.length > 0) {

                                            $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount + $scope.uploadedFiles.length;
                                            //resp.data.forEach((file) => {
                                            //    $scope.files.push(file);
                                            //    if ($scope.isItDocDacThumbnail)
                                            //        $scope.defaultImage = file.FileId;
                                            //});
                                            $scope.uppy.reset();
                                        }
                                    }
                                    , function error() { });
                            }




                        }
                        $scope.cancel();
                        $rootScope.getactor();
                    }
                }, function error() { });
            }
        }, 100)
    })
});
myApp.directive('sliderRange', ['$document', function ($document) {

    // Move slider handle and range line
    var moveHandle = function (handle, elem, posX) {
        $(elem).find('.handle.' + handle).css("left", posX + '%');
    };
    var moveRange = function (elem, posMin, posMax) {
        $(elem).find('.range').css("left", posMin + '%');
        $(elem).find('.range').css("width", posMax - posMin + '%');
    };

    return {
        template: '<div class="slider horizontal">' +
            '<div class="range"></div>' +
            '<a class="handle min" ng-mousedown="mouseDownMin($event)"></a>' +
            '<a class="handle max" ng-mousedown="mouseDownMax($event)"></a>' +
            '</div>',
        replace: true,
        restrict: 'E',
        scope: {
            valueMin: "=",
            valueMax: "="
        },
        link: function postLink(scope, element, attrs) {
            // Initilization
            var dragging = false;
            var startPointXMin = 0;
            var startPointXMax = 0;
            var xPosMin = 0;
            var xPosMax = 0;
            var settings = {
                "min": (typeof (attrs.min) !== "undefined" ? parseInt(attrs.min, 10) : 0),
                "max": (typeof (attrs.max) !== "undefined" ? parseInt(attrs.max, 10) : 100),
                "step": (typeof (attrs.step) !== "undefined" ? parseInt(attrs.step, 10) : 1)
            };
            if (typeof (scope.valueMin) == "undefined" || scope.valueMin === '')
                scope.valueMin = settings.min;

            if (typeof (scope.valueMax) == "undefined" || scope.valueMax === '')
                scope.valueMax = settings.max;

            // Track changes only from the outside of the directive
            scope.$watch('valueMin', function () {
                if (dragging) return;
                xPosMin = (scope.valueMin - settings.min) / (settings.max - settings.min) * 100;
                if (xPosMin < 0) {
                    xPosMin = 0;
                } else if (xPosMin > 100) {
                    xPosMin = 100;
                }
                moveHandle("min", element, xPosMin);
                moveRange(element, xPosMin, xPosMax);
            });

            scope.$watch('valueMax', function () {
                if (dragging) return;
                xPosMax = (scope.valueMax - settings.min) / (settings.max - settings.min) * 100;
                if (xPosMax < 0) {
                    xPosMax = 0;
                } else if (xPosMax > 100) {
                    xPosMax = 100;
                }
                moveHandle("max", element, xPosMax);
                moveRange(element, xPosMin, xPosMax);
            });

            // Real action control is here
            scope.mouseDownMin = function ($event) {
                dragging = true;
                startPointXMin = $event.pageX;

                // Bind to full document, to make move easiery (not to lose focus on y axis)
                $document.on('mousemove', function ($event) {
                    if (!dragging) return;

                    //Calculate handle position
                    var moveDelta = $event.pageX - startPointXMin;

                    xPosMin = xPosMin + ((moveDelta / element.outerWidth()) * 100);
                    if (xPosMin < 0) {
                        xPosMin = 0;
                    } else if (xPosMin > xPosMax) {
                        xPosMin = xPosMax;
                    } else {
                        // Prevent generating "lag" if moving outside window
                        startPointXMin = $event.pageX;
                    }
                    scope.valueMin = Math.round((((settings.max - settings.min) * (xPosMin / 100)) + settings.min) / settings.step) * settings.step;
                    scope.$apply();

                    // Move the Handle
                    moveHandle("min", element, xPosMin);
                    moveRange(element, xPosMin, xPosMax);
                });
                $document.mouseup(function () {
                    dragging = false;
                    $document.unbind('mousemove');
                    $document.unbind('mousemove');
                });
            };

            scope.mouseDownMax = function ($event) {
                dragging = true;
                startPointXMax = $event.pageX;

                // Bind to full document, to make move easiery (not to lose focus on y axis)
                $document.on('mousemove', function ($event) {
                    if (!dragging) return;

                    //Calculate handle position
                    var moveDelta = $event.pageX - startPointXMax;

                    xPosMax = xPosMax + ((moveDelta / element.outerWidth()) * 100);
                    if (xPosMax > 100) {
                        xPosMax = 100;
                    } else if (xPosMax < xPosMin) {
                        xPosMax = xPosMin;
                    } else {
                        // Prevent generating "lag" if moving outside window
                        startPointXMax = $event.pageX;
                    }
                    scope.valueMax = Math.round((((settings.max - settings.min) * (xPosMax / 100)) + settings.min) / settings.step) * settings.step;
                    scope.$apply();

                    // Move the Handle
                    moveHandle("max", element, xPosMax);
                    moveRange(element, xPosMin, xPosMax);
                });

                $document.mouseup(function () {
                    dragging = false;
                    $document.unbind('mousemove');
                    $document.unbind('mousemove');
                });
            };
        }
    };
}]);