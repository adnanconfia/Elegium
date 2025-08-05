

//#region Confiatech Scenes Controller
myApp.controller('scene_details_ctrl', function ($scope, $rootScope, $filter, $http, $timeout, $uibModal, toaster, $ngConfirm, $stateParams, $state, orderByFilter, uiCalendarConfig) {

    angular.element(document).ready(function () {
        $timeout(function () {

            $scope.openFileViewer = function (file, allFiles) {
                //console.log(project);
                setTimeout(function () {
                    $("[uib-modal-window='modal-window']").css('z-index', '9999999');
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
                });
                modalInstance.result.then(function () {
                }, function (data) {
                    console.log(data, 'from modal');
                    //$scope.getProjects();
                });

            }

            $scope.projectId = $stateParams.id;
            var projectId = parseInt($stateParams.id);
            var id = 0;
            if (!$stateParams.sceneId || $stateParams.sceneId == 0) {
                window.location.href = root + "#/" + projectId + "/scenesandscripts";
            } else {
                id = $stateParams.sceneId;

                var modalInstance = null;
                $scope.createshot = function () {
                    modalInstance = $uibModal.open({
                        animation: false,
                        templateUrl: root + 'js/ng-templates/scenesandscript/create-shots.html',
                        controller: 'CreateShotCtrl',
                        size: 'md',
                        backdrop: 'static',
                        resolve: {
                            title: function () {
                                return 'Create Shot ';
                            },
                            projectItem: function () {
                                return null;
                            },
                            projectId: function () {
                                return projectId;
                            },
                            sceneId: function () {
                                return $stateParams.sceneId;
                            },
                            shot: function () {
                                return null;
                            }
                        }
                    });
                }
                $scope.Editshot = function (shot) {
                    modalInstance = $uibModal.open({
                        animation: false,
                        templateUrl: root + 'js/ng-templates/scenesandscript/create-shots.html',
                        controller: 'CreateShotCtrl',
                        size: 'md',
                        backdrop: 'static',
                        resolve: {
                            title: function () {
                                return 'Create Shot ';
                            },
                            projectItem: function () {
                                return null;
                            },
                            projectId: function () {
                                return projectId;
                            },
                            sceneId: function () {
                                return $stateParams.sceneId;
                            },
                            shot: function () {
                                return shot;
                            }

                        }
                    });
                    modalInstance.result.then(function () {

                    }, function (data) {

                    });
                }

                //$scope.unitId = $stateParams.id;

                //$scope.users = [];
                //$scope.allUsers = [];
                //$scope.isEditModeUser = false;

                $scope.prev = -1;
                $scope.next = 1;

                $scope.prev_id = 0;
                $scope.next_id = 0;

                $scope.$on('allscenes', function (e) {
                    $scope.getscenes();
                    return true;
                });




                //FontAwesomeMap = {
                //    findSymbolForClass: $scope.findSymbolForClass
                //};

                /**
                 * Looks through all Stylesheets for css-selectors. Returns the content of the 
                 * first match.
                 *
                 * @param   {string} selector The complete selector or part of it 
                 *                            (e.g. 'user-md' for '.fa-user-md')
                 * @returns {string}          The content of the 'content' attribute of the 
                 *                            matching css-rule <br>
                 *                            or '' if nothing has been found
                 */
                $scope.findSymbolForClass = (selector) => {
                    var result = '';
                    var sheets = document.styleSheets;

                    for (var sheetNr = 0; sheetNr < sheets.length; sheetNr++) {
                        var content = findCSSRuleContent(sheets[sheetNr], selector);

                        if (content) {
                            result = stripQuotes(content);
                            break;
                        }
                    }

                    return result;
                }

                /**
                 * Finds the first css-rule with a selectorText containing the given selector.
                 *
                 * @param   {CSSStyleSheet} mySheet  The stylesheet to examine
                 * @param   {string}        selector The selector to match (or part of it)
                 * @returns {string}                 The content of the matching rule <br>
                 *                                   or '' if nothing has been found
                 */
                var findCSSRuleContent = function (mySheet, selector) {
                    var ruleContent = '';
                    var rules = mySheet.cssRules ? mySheet.cssRules : mySheet.rules;

                    for (var i = 0; i < rules.length; i++) {
                        var text = rules[i].selectorText;
                        if (text && text.indexOf(selector) >= 0) {
                            ruleContent = rules[i].style.content;
                            break;
                        }
                    }

                    return ruleContent;
                }

                /**
                 * Strips one leading and one trailing Character from the given String.
                 *
                 * The 'content'-Tag is assigned by a quoted String, which will als be returned 
                 * with those quotes.
                 * So we need to strip them, if we want to access the real content
                 *
                 * @param   {String} string original quoted content
                 * @returns {String}        unquoted content
                 */
                var stripQuotes = (string) => {
                    var len = string.length;
                    return string.slice(1, len - 1);
                }

                var docDefinition = {
                };



                $scope.openPdf = function () {
                    pdfMake.createPdf(docDefinition).open();
                };

                $scope.downloadPdf = function () {
                    pdfMake.createPdf(docDefinition).download($scope.project.Name + "-" + $scope.EditScene.Scene.Index + ".pdf");
                };

                var character = [];
                var display_char = (scenes) => {
                    var string = "";
                    angular.forEach(scenes, function (item) {
                        angular.forEach(item.character, function (item1) {
                            var index = character.indexOf(item1.Id) != -1;
                            if (!index) {
                                string += item1.Id + " " + item1.Name + "\n";
                                character.push(item1.Id);
                            }
                        });
                    })
                    return string;
                }
                $scope.getunits = () => {
                    $http.get(root + 'api/ProjectUnits/GetProjectUnits?ProjectId=' + projectId).then(function success(response) {

                        $scope.units = response.data;

                    }, function error() { });
                }
                var buildTableBody = () => {
                    var body = [];

                    var dataRow = [];
                    dataRow.push({ text: 'General information', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                    body.push(dataRow);

                    dataRow = [];
                    dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                    body.push(dataRow);

                    dataRow = []

                    dataRow.push({ text: $scope.EditScene.Scene.Description, style: 'small', margin: [5, 0, 0, 0], colSpan: 3 });
                    body.push(dataRow);
                    dataRow = []
                    dataRow.push({ text: [{ text: "ID\n", bold: true }, $scope.EditScene.Scene.Index], style: 'small', margin: [5, 5, 0, 0] });

                    dataRow.push({ text: [{ text: "Environment\n", bold: true }, $scope.EditScene.Env_name], style: 'small', margin: [5, 5, 0, 0] });
                    if ($scope.EditScene.Scene.ScriptDay) {
                        dataRow.push({ text: [{ text: "Script Day\n", bold: true }, $scope.EditScene.Scene.ScriptDay], style: 'small', margin: [5, 5, 0, 0] });
                    }
                    if ($scope.EditScene.Scene.ScriptPage) {
                        dataRow.push({ text: [{ text: "Script Page\n", bold: true }, $scope.EditScene.Scene.ScriptPage], style: 'small', margin: [5, 5, 0, 0] });
                    }
                    if ($scope.EditScene.Scene.ScriptPages) {
                        var temp = document.createElement('div');
                        temp.innerHTML = $scope.EditScene.Scene.ScriptPages;


                        dataRow.push({ text: [{ text: "No. of Script Pages\n", bold: true } , temp.innerText], style: 'small', margin: [5, 5, 0, 0] });
                    }
                    if ($scope.EditScene.Scene.point_in_time) {
                        dataRow.push({ text: [{ text: "Point in time\n", bold: true }, $scope.EditScene.Scene.point_in_time], style: 'small', margin: [5, 5, 0, 0] });
                    }
                    if ($scope.EditScene.Set_name) {
                        dataRow.push({ text: [{ text: "Set\n", bold: true }, $scope.EditScene.Set_name], style: 'small', margin: [5, 5, 0, 0] });
                    }
                    if ($scope.EditScene.Scene.NumberOfShots) {
                        dataRow.push({ text: [{ text: "Number of shots\n", bold: true } , $scope.EditScene.Scene.NumberOfShots], style: 'small', margin: [5, 5, 0, 0] });
                    }
                    if ($scope.EditScene.Scene.CommnityInformation) {
                        dataRow.push({ text: [{ text: "Continuity information\n", bold: true }, $scope.EditScene.Scene.CommnityInformation], style: 'small', margin: [5, 5, 0, 0] });
                    }
                    if ($scope.EditScene.Scene.scheduled_hh || $scope.EditScene.Scene.scheduled_mm) {
                        dataRow.push({
                            text: [{ text: "Scheduled duration (hh:mm)\n", bold: true }, $scope.EditScene.Scene.scheduled_hh.toLocaleString('en-US', {
                                minimumIntegerDigits: 2,
                                useGrouping: false
                            }) , ":" + $scope.EditScene.Scene.scheduled_mm.toLocaleString('en-US', {
                                minimumIntegerDigits: 2,
                                useGrouping: false
                            })], style: 'small', margin: [5, 5, 0, 0]
                        });
                    }
                    if ($scope.EditScene.Scene.Estime_mm || $scope.EditScene.Scene.Estime_ss) {
                        dataRow.push({
                            text: [{ text: "Estimated time (mm:ss)\n", bold: true } , $scope.EditScene.Scene.Estime_mm.toLocaleString('en-US', {
                                minimumIntegerDigits: 2,
                                useGrouping: false
                            }) , ":" + $scope.EditScene.Scene.Estime_ss.toLocaleString('en-US', {
                                minimumIntegerDigits: 2,
                                useGrouping: false
                            })], style: 'small', margin: [5, 5, 0, 0]
                        });
                    }
                    if ($scope.EditScene.Scene.CastType) {
                        dataRow.push({ text: [{ text: "Cast Type\n", bold: true } , $scope.EditScene.Scene.CastType], style: 'small', margin: [5, 5, 0, 0] });
                    }
                    if ($scope.unit) {
                        dataRow.push({ text: [{ text: "Unit\n", bold: true } , $scope.unit], margin: [5, 5, 0, 0] });
                    }

                    var newDataRow = [];
                    for (var k = 0; k < 15; k++) {
                        if ((k) % 3 === 0 && (k) > 0) {
                            body.push(newDataRow);
                            newDataRow = [];

                        }
                        if (k >= dataRow.length) {
                            newDataRow.push({ text: '', margin: [5, 5, 5, 5] });
                        } else {
                            newDataRow.push(dataRow[k]);
                        }
                    } body.push(newDataRow);

                    dataRow = [];
                    dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                    body.push(dataRow);


                    //Characters_PDF
                    if ($scope.EditScene.Characters && $scope.EditScene.Characters.length > 0) {
                        dataRow = [];
                        dataRow.push({ text: 'Characters', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                        dataRow = [];
                        var charadd = 0;
                        //for (i = 0; i < $scope.EditScene.Characters.length; i++) {

                        //    if ((i) % 3 == 0 && (i) > 0) {
                        //        body.push(dataRow);
                        //        dataRow = [];
                        //        charadd = 0;
                        //    }
                        //    dataRow.push({ text: $scope.EditScene.Characters[i].Index + " " + $scope.EditScene.Characters[i].Name, style: 'small' });
                        //    charadd++;

                        //}
                        angular.forEach(orderByFilter($scope.EditScene.Characters, 'Index'), function (item, i) {
                            if ((i) % 3 == 0 && (i) > 0) {
                                body.push(dataRow);
                                dataRow = [];
                                charadd = 0;
                            }
                            dataRow.push({ text: item.Index + " " + item.Name, style: 'small', margin: [5, 0, 0, 0] });
                            charadd++;
                        });

                        var charindex = (charadd - 3);
                        if (charindex < 0) {
                            charindex *= -1;
                        }
                        for (var j = 0; j < charindex; j++) {
                            dataRow.push({ text: " ", style: 'small' });

                        }
                        body.push(dataRow);


                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                    }

                    //Extras_Pdf
                    if ($scope.EditScene.extra && $scope.EditScene.extra.length > 0) {
                        dataRow = [];
                        dataRow.push({ text: 'Extras', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                        dataRow = [];
                        var extadd = 0;
                        angular.forEach(orderByFilter($scope.EditScene.extra, 'Index'), function (item, i) {

                            if ((i) % 3 == 0 && (i) > 0) {
                                body.push(dataRow);
                                dataRow = [];
                                extadd = 0;
                            }
                            dataRow.push({ text: item.Index + " " + item.Name, style: 'small' });
                            extadd++;

                        });
                        var extindex = (extadd - 3);
                        if (extindex < 0) {
                            extindex *= -1;
                        }
                        for (var j = 0; j < extindex; j++) {
                            dataRow.push({ text: " ", style: 'small' });

                        }
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                    }

                    //Costumes_pdf
                    var costumes = [];
                    angular.forEach($scope.EditScene.Characters, function (item) {
                        angular.forEach($scope.allCostumes, function (item1) {

                            if (item.Id === item1.CharacterId) {
                                costumes.push({ Name: item.Name, CName: item1.Name });
                            }
                        });
                    });
                    angular.forEach($scope.EditScene.extra, function (item) {
                        angular.forEach($scope.allCostumes, function (item1) {

                            if (item.Id === item1.ExtraId) {
                                costumes.push({ Name: item.Name, CName: item1.Name });
                            }
                        });
                    });

                    if (costumes.length > 0) {
                        dataRow = [];
                        dataRow.push({ text: 'Costumes', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                        dataRow = [];
                        charadd = 0;
                        for (i = 0; i < costumes.length; i++) {

                            if ((i) % 3 == 0 && (i) > 0) {
                                body.push(dataRow);
                                dataRow = [];
                                charadd = 0;
                            }

                            dataRow.push({ text: costumes[i].Name + ": " + costumes[i].CName, style: 'small' });
                            charadd++;

                        }
                        charindex = (charadd - 3);
                        if (charindex < 0) {
                            charindex *= -1;
                        }
                        for (var j = 0; j < charindex; j++) {
                            dataRow.push({ text: " ", style: 'small' });

                        }
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                    }


                    //Makeup_pdf
                    var Makeup = [];
                    var d = {};
                    angular.forEach($scope.EditScene.Characters, function (item) {
                        angular.forEach($scope.allMakeup, function (item1) {

                            if (item.Id === item1.CharacterId) {

                                if (!d[item.Id.toString() + "c"]) {
                                    d = {};
                                    d['Id'] = item.Id;
                                    d['Name'] = item.Name;
                                    d['type'] = "c";
                                    d[item.Id.toString() + "c"] = [item1.Name];
                                    Makeup.push(d);
                                } else {
                                    d[item.Id.toString() + "c"].push(item1.Name)
                                }
                            }
                        });
                    });
                    angular.forEach($scope.EditScene.extra, function (item) {
                        angular.forEach($scope.allMakeup, function (item1) {

                            if (item.Id === item1.ExtraId) {
                                if (!d[item.Id.toString() + "e"]) {
                                    d = {};
                                    d['Id'] = item.Id;
                                    d['Name'] = item.Name;
                                    d['type'] = "e";
                                    d[item.Id.toString() + "e"] = [item1.Name];
                                    Makeup.push(d);
                                } else {
                                    d[item.Id.toString() + "e"].push(item1.Name)
                                }
                            }
                        });
                    });

                    if (Makeup.length > 0) {
                        dataRow = [];
                        dataRow.push({ text: 'Makeups', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                        dataRow = [];
                        charadd = 0;
                        for (i = 0; i < Makeup.length; i++) {

                            if ((i) % 3 == 0 && (i) > 0) {
                                body.push(dataRow);
                                dataRow = [];
                                charadd = 0;
                            }
                            var Name = Makeup[i].Name;
                            var str = "";
                            var type = "";
                            if (Makeup[i].type == "c") {
                                type = "c";
                            } else {
                                type = "e";
                            }
                            angular.forEach(Makeup[i][Makeup[i].Id + type], function (item) {
                                str += item + ",";
                            });
                            dataRow.push({ text: Name + ": " + str, style: 'small' });
                            charadd++;

                        }
                        charindex = (charadd - 3);
                        if (charindex < 0) {
                            charindex *= -1;
                        }
                        for (var j = 0; j < charindex; j++) {
                            dataRow.push({ text: " ", style: 'small' });

                        }
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                    }

                    //construction_Pdf
                    if ($scope.EditScene.construction && $scope.EditScene.construction.length > 0) {
                        dataRow = [];
                        dataRow.push({ text: 'Constuction', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                        dataRow = [];
                        extadd = 0;
                        angular.forEach(orderByFilter($scope.EditScene.construction, 'Index'), function (item, i) {

                            if ((i) % 3 == 0 && (i) > 0) {
                                body.push(dataRow);
                                dataRow = [];
                                extadd = 0;
                            }
                            dataRow.push({ text: item.Index + " " + item.Name, style: 'small' });
                            extadd++;

                        });
                        extindex = (extadd - 3);
                        if (extindex < 0) {
                            extindex *= -1;
                        }
                        for (var j = 0; j < extindex; j++) {
                            dataRow.push({ text: " ", style: 'small' });

                        }
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                    }
                    //Dressing_Pdf
                    if ($scope.EditScene.dressing && $scope.EditScene.dressing.length > 0) {
                        dataRow = [];
                        dataRow.push({ text: 'Set Dressings', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                        dataRow = [];
                        extadd = 0;
                        angular.forEach(orderByFilter($scope.EditScene.dressing, 'Index'), function (item, i) {
                            if ((i) % 3 == 0 && (i) > 0) {
                                body.push(dataRow);
                                dataRow = [];
                                extadd = 0;
                            }
                            dataRow.push({ text: item.Index + " " + item.Name, style: 'small' });
                            extadd++;

                        });
                        extindex = (extadd - 3);
                        if (extindex < 0) {
                            extindex *= -1;
                        }
                        for (var j = 0; j < extindex; j++) {
                            dataRow.push({ text: " ", style: 'small' });

                        }
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                    }
                    //props_Pdf
                    if ($scope.EditScene.Prop && $scope.EditScene.Prop.length > 0) {
                        dataRow = [];
                        dataRow.push({ text: 'Props', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                        dataRow = [];
                        extadd = 0;
                        angular.forEach(orderByFilter($scope.EditScene.Prop, 'Index'), function (item, i) {

                            if ((i) % 3 == 0 && (i) > 0) {
                                body.push(dataRow);
                                dataRow = [];
                                extadd = 0;
                            }
                            dataRow.push({ text: item.Index + " " + item.Name, style: 'small' });
                            extadd++;

                        });
                        extindex = (extadd - 3);
                        if (extindex < 0) {
                            extindex *= -1;
                        }
                        for (var j = 0; j < extindex; j++) {
                            dataRow.push({ text: " ", style: 'small' });

                        }
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                    }
                    //Graphics_Pdf
                    if ($scope.EditScene.graphics && $scope.EditScene.graphics.length > 0) {
                        dataRow = [];
                        dataRow.push({ text: 'Graphics', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                        dataRow = [];
                        extadd = 0;
                        angular.forEach(orderByFilter($scope.EditScene.graphics, 'Index'), function (item, i) {

                            if ((i) % 3 == 0 && (i) > 0) {
                                body.push(dataRow);
                                dataRow = [];
                                extadd = 0;
                            }
                            dataRow.push({ text: item.Index + " " + item.Name, style: 'small' });
                            extadd++;

                        });
                        extindex = (extadd - 3);
                        if (extindex < 0) {
                            extindex *= -1;
                        }
                        for (var j = 0; j < extindex; j++) {
                            dataRow.push({ text: " ", style: 'small' });

                        }
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                    }
                    //Vehicle_Pdf
                    if ($scope.EditScene.vehicles && $scope.EditScene.vehicles.length > 0) {
                        dataRow = [];
                        dataRow.push({ text: 'Vehicles', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                        dataRow = [];
                        extadd = 0;
                        angular.forEach(orderByFilter($scope.EditScene.vehicles, 'Index'), function (item, i) {

                            if ((i) % 3 == 0 && (i) > 0) {
                                body.push(dataRow);
                                dataRow = [];
                                extadd = 0;
                            }
                            dataRow.push({ text: item.Index + " " + item.Name, style: 'small' });
                            extadd++;

                        });
                        extindex = (extadd - 3);
                        if (extindex < 0) {
                            extindex *= -1;
                        }
                        for (var j = 0; j < extindex; j++) {
                            dataRow.push({ text: " ", style: 'small' });

                        }
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                    }
                    //Animals_Pdf
                    if ($scope.EditScene.animals && $scope.EditScene.animals.length > 0) {
                        dataRow = [];
                        dataRow.push({ text: 'Animals', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                        dataRow = [];
                        extadd = 0;
                        for (i = 0; i < $scope.EditScene.animals.length; i++) {

                            if ((i) % 3 == 0 && (i) > 0) {
                                body.push(dataRow);
                                dataRow = [];
                                extadd = 0;
                            }
                            dataRow.push({ text: $scope.EditScene.animals[i].Name, style: 'small' });
                            extadd++;

                        }
                        extindex = (extadd - 3);
                        if (extindex < 0) {
                            extindex *= -1;
                        }
                        for (var j = 0; j < extindex; j++) {
                            dataRow.push({ text: " ", style: 'small' });

                        }
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                    }

                    //VisualEffects_Pdf
                    if ($scope.EditScene.visuals && $scope.EditScene.visuals.length > 0) {
                        dataRow = [];
                        dataRow.push({ text: 'Visual Effects', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                        dataRow = [];
                        extadd = 0;
                        for (i = 0; i < $scope.EditScene.visuals.length; i++) {

                            if ((i) % 3 == 0 && (i) > 0) {
                                body.push(dataRow);
                                dataRow = [];
                                extadd = 0;
                            }
                            dataRow.push({ text: $scope.EditScene.visuals[i].Name, style: 'small' });
                            extadd++;

                        }
                        extindex = (extadd - 3);
                        if (extindex < 0) {
                            extindex *= -1;
                        }
                        for (var j = 0; j < extindex; j++) {
                            dataRow.push({ text: " ", style: 'small' });

                        }
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                    }

                    //SpecialEffects_Pdf
                    if ($scope.EditScene.specials && $scope.EditScene.specials.length > 0) {
                        dataRow = [];
                        dataRow.push({ text: 'Special Effects', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                        dataRow = [];
                        extadd = 0;
                        for (i = 0; i < $scope.EditScene.specials.length; i++) {

                            if ((i) % 3 == 0 && (i) > 0) {
                                body.push(dataRow);
                                dataRow = [];
                                extadd = 0;
                            }
                            dataRow.push({ text: $scope.EditScene.specials[i].Name, style: 'small' });
                            extadd++;

                        }
                        extindex = (extadd - 3);
                        if (extindex < 0) {
                            extindex *= -1;
                        }
                        for (var j = 0; j < extindex; j++) {
                            dataRow.push({ text: " ", style: 'small' });

                        }
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                    }

                    //Sound_Pdf
                    if ($scope.EditScene.sounds && $scope.EditScene.sounds.length > 0) {
                        dataRow = [];
                        dataRow.push({ text: 'Sounds', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                        dataRow = [];
                        extadd = 0;
                        for (i = 0; i < $scope.EditScene.sounds.length; i++) {

                            if ((i) % 3 == 0 && (i) > 0) {
                                body.push(dataRow);
                                dataRow = [];
                                extadd = 0;
                            }
                            dataRow.push({ text: $scope.EditScene.sounds[i].Name, style: 'small' });
                            extadd++;

                        }
                        extindex = (extadd - 3);
                        if (extindex < 0) {
                            extindex *= -1;
                        }
                        for (var j = 0; j < extindex; j++) {
                            dataRow.push({ text: " ", style: 'small' });

                        }
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                    }

                    //Camera_pdf
                    if ($scope.EditScene.cameras && $scope.EditScene.cameras.length > 0) {
                        dataRow = [];
                        dataRow.push({ text: ' Cameras & Lightings', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                        dataRow = [];
                        extadd = 0;
                        for (i = 0; i < $scope.EditScene.cameras.length; i++) {

                            if ((i) % 3 == 0 && (i) > 0) {
                                body.push(dataRow);
                                dataRow = [];
                                extadd = 0;
                            }
                            dataRow.push({ text: $scope.EditScene.cameras[i].Name, style: 'small' });
                            extadd++;

                        }
                        extindex = (extadd - 3);
                        if (extindex < 0) {
                            extindex *= -1;
                        }
                        for (var j = 0; j < extindex; j++) {
                            dataRow.push({ text: " ", style: 'small' });

                        }
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                    }
                    //Stunts_pdf
                    if ($scope.EditScene.stunts && $scope.EditScene.stunts.length > 0) {
                        dataRow = [];
                        dataRow.push({ text: ' Stunts', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                        dataRow = [];
                        extadd = 0;
                        for (i = 0; i < $scope.EditScene.stunts.length; i++) {

                            if ((i) % 3 == 0 && (i) > 0) {
                                body.push(dataRow);
                                dataRow = [];
                                extadd = 0;
                            }
                            dataRow.push({ text: $scope.EditScene.stunts[i].Name, style: 'small' });
                            extadd++;

                        }
                        extindex = (extadd - 3);
                        if (extindex < 0) {
                            extindex *= -1;
                        }
                        for (var j = 0; j < extindex; j++) {
                            dataRow.push({ text: " ", style: 'small' });

                        }
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                    }

                    //Others_pdf
                    if ($scope.EditScene.others && $scope.EditScene.others.length > 0) {
                        dataRow = [];
                        dataRow.push({ text: ' Others', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                        dataRow = [];
                        extadd = 0;
                        for (i = 0; i < $scope.EditScene.others.length; i++) {

                            if ((i) % 3 == 0 && (i) > 0) {
                                body.push(dataRow);
                                dataRow = [];
                                extadd = 0;
                            }
                            dataRow.push({ text: $scope.EditScene.others[i].Name, style: 'small' });
                            extadd++;

                        }
                        extindex = (extadd - 3);
                        if (extindex < 0) {
                            extindex *= -1;
                        }
                        for (var j = 0; j < extindex; j++) {
                            dataRow.push({ text: " ", style: 'small' });

                        }
                        body.push(dataRow);
                        dataRow = [];
                        dataRow.push({ text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                        body.push(dataRow);
                    }


                    return body;
                }
                getBase64ImageFromUrl = (url) => {
                    return new Promise((resolve, reject) => {
                        var img = new Image();
                        img.setAttribute("crossOrigin", "anonymous");
                        img.onload = () => {
                            var canvas = document.createElement("canvas");
                            canvas.width = img.width;
                            canvas.height = img.height;
                            var ctx = canvas.getContext("2d");
                            ctx.drawImage(img, 0, 0);
                            var dataURL = canvas.toDataURL("image/png");
                            resolve(dataURL);
                        };
                        img.onerror = error => {
                            reject(error);
                        };
                        img.src = url;
                    });
                }
                //FIlespdf
                var getfiles = () => {

                    var body = [];
                    if ($scope.files.length > 0) {
                        //dataRow = [];
                        //dataRow.push({ text: ' Others', fillColor: '#E6E6E6', bold: true, fontSize: 12 }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                        //body.push(dataRow);
                        var dataRow = [];
                        extadd = 0;
                        var i = 0;
                        for (i = 0; i < $scope.files.length; i++) {
                            var img = window.location.protocol + "//" + window.location.host + '/api/UserProfiles/GetFileThumbnail/' + $scope.files[i].FileId + '/200/200';

                            if ((i) % 4 == 0 && (i) > 0) {
                                body.push(dataRow);
                                dataRow = [];
                                extadd = 0;
                            }

                            //var img = await getBase64ImageFromUrl('/api/UserProfiles/GetFileThumbnail/' + $scope.files[i].FileId + '/200/200');                       




                            dataRow.push({
                                image: img,
                                Id: i.toString()
                            });

                            extadd++;
                            extindex = (extadd - 3);
                            if (extindex < 0) {
                                extindex *= -1;
                            }


                        }
                        if (dataRow.length > 0) {
                            body.push(dataRow);
                        }
                    }


                    return (body);

                }
                var buildShotHeader = () => {
                    var body = [];
                    if ($scope.EditScene.Shots && $scope.EditScene.Shots.length > 0) {
                        var dataRow = [];
                        dataRow.push({ text: ' Shots', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                        body.push(dataRow)
                        dataRow = [];
                        dataRow.push({ text: ' Shot' }, { text: "Description" }, { text: "Size" }, { text: "Type" }, { text: "Movement" }, { text: "Equipment" }, { text: "Other" });
                        body.push(dataRow)
                    }
                    return body;
                }
                var buildShotBody = () => {
                    var body = [];
                    if ($scope.EditScene.Shots && $scope.EditScene.Shots.length > 0) {
                        var dataRow = [];



                        for (i = 0; i < $scope.EditScene.Shots.length; i++) {

                            dataRow = [];
                            dataRow.push({ text: $scope.EditScene.Shots[i].Index, style: 'small' });
                            dataRow.push({
                                text: $scope.EditScene.Shots[i].Subject + "\n" + $scope.EditScene.Shots[i].Visual
                                , style: 'small'
                            });
                            dataRow.push({
                                text: $scope.EditScene.Shots[i].Size.Name
                                , style: 'small'
                            });
                            var str = "";
                            for (var j = 0; j < $scope.EditScene.Shots[i].Type.length; j++) {
                                str += $scope.EditScene.Shots[i].Type[j].Name + "/";
                            }
                            dataRow.push({
                                text: str
                                , style: 'small'
                            });
                            dataRow.push({
                                text: $scope.EditScene.Shots[i].Movement.Name
                                , style: 'small'
                            });
                            str = "";
                            for (var j = 0; j < $scope.EditScene.Shots[i].Equipment.length; j++) {
                                str += $scope.EditScene.Shots[i].Equipment[j].Name + "/";
                            }
                            dataRow.push({
                                text: str
                                , style: 'small'
                            });
                            str = "";
                            for (var j = 0; j < $scope.EditScene.Shots[i].Vfx.length; j++) {
                                str += $scope.EditScene.Shots[i].Vfx[j].Name + "/";
                            }
                            for (var j = 0; j < $scope.EditScene.Shots[i].Camera.length; j++) {
                                str += $scope.EditScene.Shots[i].Camera[j].Name + "/";
                            }
                            for (var j = 0; j < $scope.EditScene.Shots[i].Lens.length; j++) {
                                str += $scope.EditScene.Shots[i].Lens[j].Name + "/";
                            }
                            for (var j = 0; j < $scope.EditScene.Shots[i].SpecialEquipment.length; j++) {
                                str += $scope.EditScene.Shots[i].SpecialEquipment[j].Name + "/";
                            }
                            dataRow.push({
                                text: str
                                , style: 'small'
                            });
                            body.push(dataRow);
                        }


                    }

                    return body;
                }
                $scope.getProject = () => {
                    $http.get(root + 'api/Projects/GetProject/' + projectId).then(function success(response) {

                        console.log(response, "projects");
                        $scope.project = response.data.Project;
                        var today = new Date();
                        var files = getfiles();
                        var images = {};
                        for (var i = 0; i < files.length; i++) {
                            for (var j = 0; j < files[i].length; j++) {
                                var id = files[i][j].Id.toString();
                                var data = files[i][j].image.toString();
                                images[id] = data;
                            }
                        }
                        var columns = [];
                        for (var i = 0; i < files.length; i++) {
                            var colcell = [];
                            for (var j = 0; j < files[i].length; j++) {
                                var id = files[i][j].Id.toString();
                                var data = files[i][j].image.toString();
                                images[id] = data;
                                colcell.push({ image: id, width: 100, margin: [0, 20] })
                            }
                            columns.push({ columnGap: 20, columns: colcell });
                        }

                        today = $filter('date')(today, "EEEE , MMMM dd,yyyy hh:mm a");
                        var shotsHeader = null;
                        var shotBody = null;
                        if ($scope.EditScene.Shots.length > 0) {
                            shotsHeader = {

                                alignment: 'justify',
                                margin: [0, 0, 0, 10],

                                style: 'table',
                                table: {
                                    margin: [0, 0, 0, 10],
                                    widths: ["14%", "14%", "14%", "14%", "14%", "14%", "14%"],
                                    body: buildShotHeader()
                                    //    [

                                    //    [{ colSpan: 3, text: 'General Information ' ,fillColor:'#ccc' }],
                                    //    [{ text: 'a' }, { text: 'a' },{ text: 'a' }],

                                    //]


                                }, layout: 'noBorders'
                            }
                            shotBody = {

                                alignment: 'justify',
                                margin: [0, 0, 0, 10],

                                style: 'table',
                                table: {
                                    margin: [0, 0, 0, 10],
                                    widths: ["14%", "14%", "14%", "14%", "14%", "14%", "14%"],
                                    body: buildShotBody()
                                    //    [

                                    //    [{ colSpan: 3, text: 'General Information ' ,fillColor:'#ccc' }],
                                    //    [{ text: 'a' }, { text: 'a' },{ text: 'a' }],

                                    //]


                                },



                            }
                        }
                        docDefinition = {
                            content: [

                                {
                                    alignment: 'justify',
                                    margin: [0, 0, 0, 50],
                                    columns: [
                                        {
                                            width: 90,
                                            alignment: 'left',

                                            text: ""

                                        },
                                        {
                                            width: '*',
                                            alignment: 'left',

                                            text: [$scope.project.Name + "\n", { text: "Scene " + $scope.EditScene.Scene.Index + "\n", bold: true }, { text: today, style: 'small', bold: true }
                                            ]
                                        },

                                        {
                                            style: 'small',
                                            width: '*',
                                            text: [

                                                { text: $scope.findSymbolForClass('.fa-user'), style: 'symbol', bold: false },
                                                " " + $scope.project.ContactName + "\n",
                                                { text: $scope.findSymbolForClass('.fa-phone'), style: 'symbol', bold: false },
                                                " " + $scope.project.ContactPhone + "\n",
                                                { text: $scope.findSymbolForClass('.fa-envelope'), style: 'symbol', bold: false },
                                                " " + $scope.project.ContactEmail]
                                        },

                                    ]

                                },

                                {

                                    alignment: 'justify',
                                    margin: [0, 0, 0, 10],

                                    style: 'table',
                                    table: {
                                        margin: [0, 0, 0, 10],
                                        widths: ["33%", "33%", "33%"],
                                        body: buildTableBody()
                                        //    [

                                        //    [{ colSpan: 3, text: 'General Information ' ,fillColor:'#ccc' }],
                                        //    [{ text: 'a' }, { text: 'a' },{ text: 'a' }],

                                        //]


                                    },


                                    layout: 'noBorders'
                                },
                                columns,

                                shotsHeader,

                                shotBody


                            ],
                            images: images,

                            styles: {
                                header: {
                                    bold: true,
                                    color: '#000',
                                    fontSize: 11
                                },
                                small: {
                                    font: 'Roboto',
                                    fontSize: 9
                                },
                                symbol: {
                                    font: 'FontAwesome'
                                },
                                txt_left: {
                                    alignment: 'left',
                                }, table: {
                                    margin: [0, 5, 0, 15],
                                    fontSize: 9,


                                }
                            }
                        }
                    }, function error() { });
                }


                $rootScope._getscenes = () => {
                    $scope.getscenes();
                };

                $scope.getscenes = function () {
                    $http.get(root + 'api/ScenesandScript/GetScenes?projectId=' + projectId).then(function success(response) {

                        console.log(response);
                        $scope.scenes = response.data;
                        if ($scope.scenes.length == 0) {
                            $rootScope.getscenes();
                            window.location.href = root + "#/" + projectId + "/scenesandscripts";
                        } else {
                            $rootScope.getscenes();
                            $scope.getenv();
                            $scope.SceneDetail();
                            getCostumes();
                            getMakeup();
                            $scope.getcharacters();
                            $scope.getextra();

                            $scope.getdressing();
                            $scope.getProps();
                            $scope.getvehicle();
                            $scope.getanimal();
                            $scope.getgraphic();
                            $scope.getvisual();
                            $scope.getspecial();
                            $scope.getcamera();
                            $scope.getstunt();
                            $scope.getother();
                            $scope.getsound();
                            $scope.getSceneFiles();
                            $scope.getProjectUnits();
                            $scope.getConstruction();
                            initializeTaskObj();

                            $scope.getComments();
                            $scope.comments = [];

                            $scope.commentObj = {
                                Text: "",
                                newComment: true,
                                AnnouncementId: null,
                                MentionUsers: null,
                                SceneId: parseInt($scope.scene_id)
                            };
                            $scope.mytasks = [];
                            $scope.getProject();
                        }
                    }, function error() { });
                }

                $scope.getscenes();
                $scope.getenv = function () {
                    $http.get(root + 'api/ScenesandScript/GetEnvironment?projectid=' + projectId).then(function success(response) {


                        $scope.env = response.data;
                        angular.forEach($scope.env, function (item) {

                            if (item.Id == $scope.EditScene.Scene.environment_id) {

                                $scope.environment = item;
                            }

                        });
                    }, function error() { });
                }
                $scope.createScene = () => {
                    var Scene = {
                        project_id: projectId,
                        ScriptPages: "-",
                        Estime_mm: 0,
                        Estime_ss: 0,
                        scheduled_hh: 0,
                        scheduled_mm: 0,
                        environment_id: 1,
                        ScriptDay: 0,
                        ScriptPage: 0,
                        NumberOfShots: 0,
                        IsOmitted: false,
                        isDeleted: false,
                        Index: (parseInt(id) + 1).toString()


                        //,

                        //Index: Index,
                        //environment_id: environment_id,
                        //point_in_time: point_in_time,
                        //setId: setId,
                        //Description: Description

                    };
                    var allscenes = {
                        "ExtraId": [],
                        "CharacterId": [],
                        "Id": 0,
                        "scene": Scene


                    }


                    $http.post(root + 'api/ScenesandScript/CreateScene/', (allscenes)
                    ).then(response => {
                        if (response.status == 200) {
                            toaster.pop({
                                type: 'success',
                                title: 'Success',
                                body: 'Scene created successfully!',
                            });
                            $rootScope.getscenes();
                            $scope.getscenes();
                            $timeout(function () { $scope.nextScene(); }, 100);

                        }
                    }, err = () => { });

                }
                $scope.cnfrm = false;
                $scope.duplicate = () => {
                    $scope.cnfrm = true;

                }
                $scope.yes = (id) => {
                    $http.get(root + 'api/ScenesandScript/DuplicateScene?id=' + id).then(function success(response) {
                        $scope.cnfrm = false;

                        $scope.$broadcast('allscenes');
                        if (response.status == 200) {
                            toaster.pop({
                                type: 'success',
                                title: 'Success',
                                body: 'Duplicate Scene created successfully!',
                            });

                        }
                        $rootScope.getscenes();
                        return true;
                    }, function error() { });
                }
                $scope.No = () => {
                    $scope.cnfrm = false;
                }
                $scope.SceneDetail = () => {
                    $scope.EditScene = new Object();

                    $scope.scene_id = id;
                    var i = 0;
                    angular.forEach($scope.scenes, function (item) {

                        if (item.Id == id) {

                            $scope.SceneIndex = i;
                        }
                        i++;
                    });

                    if ($scope.SceneIndex == $scope.scenes.length - 1) {
                        $scope.next = -1;
                        $scope.next_id = 0;
                    } else {
                        $scope.next = $scope.SceneIndex + 1;
                        $scope.next_id = $scope.scenes[$scope.SceneIndex + 1].Id
                    }
                    if ($scope.SceneIndex == 0) {
                        $scope.prev = -1;
                        $scope.prev_id = 0;
                    } else {
                        $scope.prev = $scope.SceneIndex - 1;
                        $scope.prev_id = $scope.scenes[$scope.SceneIndex - 1].Id
                    }


                    $scope.EditScene.Id = id;

                    $scope.EditScene.Characters = $scope.scenes[$scope.SceneIndex].character;
                    $scope.EditScene.extra = $scope.scenes[$scope.SceneIndex].extra;

                    $scope.EditScene.Scene = $scope.scenes[$scope.SceneIndex].scene;
                    $scope.EditScene.Env_name = $scope.scenes[$scope.SceneIndex].Env_name;
                    $scope.EditScene.Set_name = $scope.scenes[$scope.SceneIndex].Set_name;
                    $scope.EditScene.CharacterId = $scope.scenes[$scope.SceneIndex].CharacterId;
                    $scope.EditScene.ExtraId = $scope.scenes[$scope.SceneIndex].ExtraId;
                    $scope.EditScene.construction = $scope.scenes[$scope.SceneIndex].construction;
                    $scope.EditScene.dressing = $scope.scenes[$scope.SceneIndex].dressings;
                    $scope.EditScene.Prop = $scope.scenes[$scope.SceneIndex].Prop;
                    $scope.EditScene.graphics = $scope.scenes[$scope.SceneIndex].graphics;
                    $scope.EditScene.vehicles = $scope.scenes[$scope.SceneIndex].vehicles;
                    $scope.EditScene.animals = $scope.scenes[$scope.SceneIndex].animals;
                    $scope.EditScene.visuals = $scope.scenes[$scope.SceneIndex].visualEffects;
                    $scope.EditScene.specials = $scope.scenes[$scope.SceneIndex].specialEffects;
                    $scope.EditScene.sounds = $scope.scenes[$scope.SceneIndex].sound;
                    $scope.EditScene.cameras = $scope.scenes[$scope.SceneIndex].cameras;
                    $scope.EditScene.stunts = $scope.scenes[$scope.SceneIndex].stunts;
                    $scope.EditScene.others = $scope.scenes[$scope.SceneIndex].others;
                    $scope.EditScene.costumes = $scope.scenes[$scope.SceneIndex].costumes;
                    $scope.EditScene.makeups = $scope.scenes[$scope.SceneIndex].makeups;
                    $scope.EditScene.Shots = $scope.scenes[$scope.SceneIndex].Shots;
                    if ($scope.EditScene.Scene.ScriptPages === '-') {
                        $scope.EditScene.Scene.ScriptPages = "";
                    }

                }
                $scope.pagelist = true;
                $scope.unitlist = true;
                var list_check = true;
                var unit_list_check = true;

                $scope.pages = ["0", "1/8", "2/8", "3/8", "4/8", "5/8", "6/8", "7/8", "1",
                    "1 <sup>1/8</sup>",
                    "1 <sup>2/8</sup>",
                    "1 <sup>3/8</sup>",
                    "1 <sup>4/8</sup>",
                    "1 <sup>5/8</sup>",
                    "1 <sup>6/8</sup>",
                    "1 <sup>7/8</sup>",
                    "2",
                    "2 <sup>1/8</sup>",
                    "2 <sup>2/8</sup>",
                    "2 <sup>3/8</sup>",
                    "2 <sup>4/8</sup>",
                    "2 <sup>5/8</sup>",
                    "2 <sup>6/8</sup>",
                    "2 <sup>7/8</sup>",
                    "3",
                    "3 <sup>1/8</sup>",
                    "3 <sup>2/8</sup>",
                    "3 <sup>3/8</sup>",
                    "3 <sup>4/8</sup>",
                    "3 <sup>5/8</sup>",
                    "3 <sup>6/8</sup>",
                    "3 <sup>7/8</sup>",
                    "4",
                    "4 <sup>1/8</sup>",
                    "4 <sup>2/8</sup>",
                    "4 <sup>3/8</sup>",
                    "4 <sup>4/8</sup>",
                    "4 <sup>5/8</sup>",
                    "4 <sup>6/8</sup>",
                    "4 <sup>7/8</sup>",
                    "5",
                    "5 <sup>1/8</sup>",
                    "5 <sup>2/8</sup>",
                    "5 <sup>3/8</sup>",
                    "5 <sup>4/8</sup>",
                    "5 <sup>5/8</sup>",
                    "5 <sup>6/8</sup>",
                    "5 <sup>7/8</sup>",
                    "6",
                    "6 <sup>1/8</sup>",
                    "6 <sup>2/8</sup>",
                    "6 <sup>3/8</sup>",
                    "6 <sup>4/8</sup>",
                    "6 <sup>5/8</sup>",
                    "6 <sup>6/8</sup>",
                    "6 <sup>7/8</sup>",
                    "7",
                    "7 <sup>1/8</sup>",
                    "7 <sup>2/8</sup>",
                    "7 <sup>3/8</sup>",
                    "7 <sup>4/8</sup>",
                    "7 <sup>5/8</sup>",
                    "7 <sup>6/8</sup>",
                    "7 <sup>7/8</sup>",
                    "8",
                    "8 <sup>1/8</sup>",
                    "8 <sup>2/8</sup>",
                    "8 <sup>3/8</sup>",
                    "8 <sup>4/8</sup>",
                    "8 <sup>5/8</sup>",
                    "8 <sup>6/8</sup>",
                    "8 <sup>7/8</sup>",
                    "9",
                    "9 <sup>1/8</sup>",
                    "9 <sup>2/8</sup>",
                    "9 <sup>3/8</sup>",
                    "9 <sup>4/8</sup>",
                    "9 <sup>5/8</sup>",
                    "9 <sup>6/8</sup>",
                    "9 <sup>7/8</sup>",
                    "10",
                    "10 <sup>1/8</sup>",
                    "10 <sup>2/8</sup>",
                    "10 <sup>3/8</sup>",
                    "10 <sup>4/8</sup>",
                    "10 <sup>5/8</sup>",
                    "10 <sup>6/8</sup>",
                    "10 <sup>7/8</sup>",
                    "11",
                    "11 <sup>1/8</sup>",
                    "11 <sup>2/8</sup>",
                    "11 <sup>3/8</sup>",
                    "11 <sup>4/8</sup>",
                    "11 <sup>5/8</sup>",
                    "11 <sup>6/8</sup>",
                    "11 <sup>7/8</sup>",
                    "12",
                    "12 <sup>1/8</sup>",
                    "12 <sup>2/8</sup>",
                    "12 <sup>3/8</sup>",
                    "12 <sup>4/8</sup>",
                    "12 <sup>5/8</sup>",
                    "12 <sup>6/8</sup>",
                    "12 <sup>7/8</sup>",
                    "13",
                    "13 <sup>1/8</sup>",
                    "13 <sup>2/8</sup>",
                    "13 <sup>3/8</sup>",
                    "13 <sup>4/8</sup>",
                    "13 <sup>5/8</sup>",
                    "13 <sup>6/8</sup>",
                    "13 <sup>7/8</sup>",
                    "14",
                    "14 <sup>1/8</sup>",
                    "14 <sup>2/8</sup>",
                    "14 <sup>3/8</sup>",
                    "14 <sup>4/8</sup>",
                    "14 <sup>5/8</sup>",
                    "14 <sup>6/8</sup>",
                    "14 <sup>7/8</sup>",
                    "15",




                ];

                $scope.complete = function (string) {
                    //$scope.filtered_pages = null;
                    if (string == null || string == "undefined") {
                        $scope.pagelist = true;
                    } else {
                        $scope.pagelist = true;
                        var output = [];
                        angular.forEach($scope.pages, function (item) {
                            if (item.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                output.push(item);
                            }
                        });
                        if (output.length > 0) {
                            $scope.pagelist = false;
                        }
                        else {
                            $scope.pagelist = true;
                        }
                        $scope.filtered_pages = output;
                    }
                }
                $scope.view_list = () => {
                    if (list_check === true) {
                        list_check = false;
                        $scope.filtered_pages = null;
                        $scope.filtered_pages = $scope.pages;
                        $scope.pagelist = false;
                    }
                    else {
                        list_check = true;
                        $scope.filtered_pages = null;
                        $scope.pagelist = true;
                    }

                }
                $scope.fillTextbox = function (string) {
                    $scope.EditScene.Scene.ScriptPages = string;
                    $scope.filtered_pages = null;
                    $scope.pagelist = true;
                }

                //SET
                $scope.getset = function () {
                    $http.get(root + 'api/ScenesandScript/GetSets?projectid=' + projectId).then(function success(response) {


                        $scope.set = response.data;
                        $scope.filtered_set = response.data;
                    }, function error() { });
                }
                $scope.getset();
                $scope.show_set_check = true;
                $scope.toggle_set = () => {
                    if ($scope.show_set_check) {
                        $scope.show_set_check = false;
                    } else {
                        $scope.show_set_check = true;
                    }
                }

                $scope.setbtn = true;
                $scope.set_set = (Id, Set_name) => {
                    $scope.show_set_check = true;
                    $scope.EditScene.Scene.setId = Id;
                    $scope.EditScene.Set_name = Set_name;
                }
                $scope.createSet = (set) => {
                    var newset = {
                        "Set_name": set,
                        "ProjectId": projectId
                    }
                    $http.post(root + 'api/ScenesandScript/CreateSet', newset).then(
                        function success(resp) {
                            if (resp.status === 200) {
                                $scope.getset();
                                $scope.setbtn = true;
                            }
                        }, function error() { });

                }
                $scope.complete_set = function (e, string) {
                    //$scope.filtered_pages = null;
                    if (!string) {
                        $scope.show_set_check = false;
                        $scope.setbtn = true;
                        $scope.filtered_set = $scope.set;
                    } else if (e.keyCode === 13) {
                        $scope.createSet(string);
                    }
                    else {
                        $scope.show_set_check = false;
                        var output = [];
                        angular.forEach($scope.set, function (item) {
                            if (item.Set_name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                output.push(item);
                            }
                        });
                        if (output.length > 0) {
                            $scope.show_set_check = false;
                            $scope.setbtn = true;
                        }
                        else {
                            $scope.show_set_check = false;
                            $scope.setbtn = false;
                        }
                        $scope.filtered_set = output;
                    }
                }
                //Characters
                $scope.charbtn = true;
                $scope.charIndex = -1;

                $scope.hideSplit = () => {
                    $scope.charIndex = -1;
                }
                $scope.showDel = (id, type) => {
                    $scope.charIndex = id + type;
                }
                $scope.getcharacters = function () {
                    $http.get(root + 'api/ScenesandScript/GetCharacters?projectid=' + projectId).then(function success(response) {

                        console.log(response);
                        $scope.chars = response.data;

                        $scope.filtered_chars = $scope.chars.filter(function (item) {
                            var exist = $scope.EditScene.CharacterId.includes(item.Id.toString());

                            if (!exist) {
                                //console.log(exist, item.Id);
                                return item;
                            }
                        });
                        console.log($scope.chars);



                    }, function error() { });
                }
                $scope.createchar = (ch) => {

                    var newch = {
                        "Name": ch,

                        "Project_Id": projectId
                    }
                    $scope.char_srch = "";
                    $http.post(root + 'api/ScenesandScript/CreateChar', newch).then(
                        function success(resp) {
                            if (resp.status === 200) {
                                var item = resp.data;
                                $scope.chars.push(item);
                                item.index = $scope.chars.length;
                                $scope.filtered_chars = $scope.chars;
                                var arrayWithIds = $scope.EditScene.Characters.map(function (x) {
                                    return x.Id;
                                });

                                $scope.filtered_chars = $scope.filtered_chars.filter(function (item) {
                                    var present = arrayWithIds.indexOf(item.Id) != -1;
                                    if (!present) {
                                        return item;
                                    }

                                });
                                $scope.charbtn = true;
                                $rootScope.GetOnBoarding();
                            }
                        }, function error() { });

                }
                $scope.complete_char = function (e, string) {
                    //$scope.filtered_pages = null;
                    if (string == null || string == "undefined") {
                        $scope.char_to_add = false;
                        $scope.charbtn = true;
                        //$scope.filtered_chars = $scope.chars;
                    } else if (e.keyCode === 13) {
                        $scope.createchar(string);
                    } else {

                        var output = [];
                        var chk = false;
                        angular.forEach($scope.chars, function (item) {

                            if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                output.push(item);
                                var exist = $scope.EditScene.CharacterId.includes(item.Id.toString());

                                if (exist) {
                                    chk = true;

                                }
                            }
                        });
                        if (output.length > 0 && !chk) {
                            $scope.charbtn = true;
                            $scope.filtered_chars = output;
                        } else if (output.length == 0) {
                            $scope.filtered_chars = output;
                            $scope.charbtn = false;
                        }
                        else if (chk) {
                            $scope.charbtn = true;
                        }
                        else {
                            $scope.charbtn = false;
                        }

                    }
                }
                $scope.add_char_check = true;
                $scope.char_to_add = true;


                $scope.show_add_char = () => {
                    $scope.char_to_add = true;
                }
                $scope.show_char = () => {
                    $scope.char_to_add = false;
                }
                $scope.add_char = (id) => {
                    //$scope.filtered_chars.forEach(function (item) {
                    //    //console.log(item)
                    //    if (item.Id == id) {

                    //        $scope.addedchar.push(item);
                    //    }
                    //});
                    $scope.filtered_chars = $scope.filtered_chars.filter(function (item) {
                        if (item.Id != id) {
                            return item;
                        }
                    });

                    //console.log($scope.addedchar);

                    if ($scope.filtered_chars.length > 0) {
                        $scope.char_to_add = false;
                    } else {
                        $scope.char_to_add = true;
                    }
                    var scenecharacter = {
                        "SceneId": parseInt($scope.EditScene.Id),
                        "CharacterId": id
                    }
                    $http.post(root + 'api/ScenesandScript/AddCharacter/', (scenecharacter)
                    ).then(response => {
                        if (response.status == 200) {
                            $scope.getscenes();
                        }
                    }, err => {

                    });

                }


                //extra


                $scope.extbtn = true;
                $scope.getextra = function () {
                    $http.get(root + 'api/ScenesandScript/GetExtras?projectid=' + projectId).then(function success(response) {

                        console.log(response);
                        $scope.ext = response.data;

                        $scope.filtered_ext = $scope.ext.filter(function (item) {
                            var exist = $scope.EditScene.ExtraId.includes(item.Id.toString());

                            if (!exist) {
                                //console.log(exist, item.Id);
                                return item;
                            }
                        });
                        //console.log($scope.ext);


                    }, function error() { });
                }
                $scope.createext = (ch) => {
                    var newch = {
                        "Name": ch,
                        "Project_Id": projectId
                    }
                    $scope.ext_srch = "";
                    $http.post(root + 'api/ScenesandScript/Createext', newch).then(
                        function success(resp) {
                            if (resp.status === 200) {
                                var item = resp.data;
                                $scope.ext.push(item);
                                item.index = $scope.ext.length;
                                $scope.filtered_ext = $scope.ext;

                                var arrayWithIds = $scope.EditScene.extra.map(function (x) {
                                    return x.Id;
                                });

                                $scope.filtered_ext = $scope.filtered_ext.filter(function (item) {
                                    var present = arrayWithIds.indexOf(item.Id) != -1;
                                    if (!present) {
                                        return item;
                                    }

                                });


                                $scope.extbtn = true;
                            }
                        }, function error() { });

                }
                $scope.complete_ext = function (e, string) {
                    //$scope.filtered_pages = null;
                    if (string == null || string == "undefined") {
                        $scope.ext_to_add = false;
                        $scope.extbtn = true;
                        //$scope.filtered_ext = $scope.ext;
                    } else if (e.keyCode === 13) {
                        $scope.createext(string);
                    } else {

                        var output = [];
                        var chk = false;
                        angular.forEach($scope.ext, function (item) {

                            if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                output.push(item);
                                var exist = $scope.EditScene.ExtraId.includes(item.Id.toString());

                                if (exist) {
                                    chk = true;

                                }
                            }
                        });
                        if (output.length > 0 && !chk) {
                            $scope.extbtn = true;
                            $scope.filtered_ext = output;
                        } else if (output.length == 0) {
                            $scope.filtered_ext = output;
                            $scope.extbtn = false;
                        }
                        else if (chk) {
                            $scope.extbtn = true;
                        }
                        else {
                            $scope.extbtn = false;
                        }

                    }
                }
                $scope.add_ext_check = true;
                $scope.ext_to_add = true;





                $scope.show_add_ext = () => {
                    $scope.ext_to_add = true;
                }
                $scope.show_ext = () => {
                    $scope.ext_to_add = false;
                }
                $scope.add_ext = (id) => {
                    //$scope.filtered_chars.forEach(function (item) {
                    //    //console.log(item)
                    //    if (item.Id == id) {

                    //        $scope.addedchar.push(item);
                    //    }
                    //});
                    $scope.filtered_ext = $scope.filtered_ext.filter(function (item) {
                        if (item.Id != id) {
                            return item;
                        }
                    });

                    //console.log($scope.addedchar);

                    if ($scope.filtered_ext.length > 0) {
                        $scope.ext_to_add = false;
                    } else {
                        $scope.ext_to_add = true;
                    }
                    var sceneExtra = {
                        "SceneId": parseInt($scope.EditScene.Id),
                        "ExtraId": id
                    }
                    $http.post(root + 'api/ScenesandScript/AddExtra/', (sceneExtra)
                    ).then(response => {
                        if (response.status == 200) {
                            $scope.getscenes();
                        }
                    }, err => {

                    });

                }
                $scope.Color = function (color) {
                    colorInt = parseInt(color.slice(1), 16);
                    otherColor = colorInt ^ 0x1FFFFFF;
                    //console.log(colorInt.toString(16), otherColor.toString(16));
                    return {
                        background: color,
                        color: '#' + otherColor.toString(16).slice(1)
                    };
                }
                $scope.removeExt = function (id) {
                    var data = {
                        "ExtraId": id,
                        "SceneId": parseInt($scope.scene_id)

                    }
                    $http.post(root + 'api/ScenesandScript/DeleteExtra', data).then(function success(response) {
                        //console.log(response);

                        $scope.getscenes();


                    }, function error() { });
                }

                //Costumes
                $scope.CostumeIndex = -1;
                $scope.show_costumes = function (index, char) {
                    $scope.CostumeIndex = index + char;
                }
                $scope.hide_costumes = function () {
                    $scope.CostumeIndex = -1;
                }

                $scope.checkCostume = (id, type) => {
                    if ($scope.allCostumes) {
                        if (type == 0) {
                            var arrayWithIds = $scope.allCostumes.map(function (x) {
                                return x.CharacterId;
                            });
                            var present = arrayWithIds.indexOf(id) != -1
                            //console.log(present,id,type);
                            return present

                        } else {
                            var arrayWithIds = $scope.allCostumes.map(function (x) {
                                return x.ExtraId;
                            });
                            var present = arrayWithIds.indexOf(id) != -1
                            //console.log(present, id, type);
                            return present

                            //$scope.Costume = null;

                        }
                    }
                }
                var getCostumes = () => {
                    $http.get(root + 'api/ScenesandScript/GetCostumes?projectId=' + projectId).then(function (response) {
                        //console.log(response);
                        $scope.costumes = response.data;
                        $scope.allCostumes = response.data;

                        var arrayWithIds = $scope.EditScene.costumes.map(function (x) {
                            return x.Costumes.Id;
                        });
                        output = [];
                        $scope.filtered_costumes = $scope.costumes.filter(function (item) {
                            var present = arrayWithIds.indexOf(item.Id) != -1
                            //if (!$scope.EditScene.costumes[arrayWithIds.indexOf(item.Id)].CharacterId) {
                            //    Cid = "";
                            //} else {
                            //    Cid = $scope.EditScene.costumes[arrayWithIds.indexOf(item.Id)].CharacterId;
                            //}
                            //if (!$scope.EditScene.costumes[arrayWithIds.indexOf(item.Id)].ExtraId) {
                            //    eid = "";
                            //} else {
                            //    eid = $scope.EditScene.costumes[arrayWithIds.indexOf(item.Id)].ExtraId;
                            //}
                            //var cp = $scope.EditScene.CharacterId.indexOf(Cid.toString()) != -1;
                            //var ex = $scope.EditScene.ExtraId.indexOf(eid.toString()) != -1;
                            //console.log(present, id, type);
                            if (!present) {
                                return item;
                            } else {
                                item.ExtraId = $scope.EditScene.costumes[arrayWithIds.indexOf(item.Id)].ExtraId;
                                item.CharacterId = $scope.EditScene.costumes[arrayWithIds.indexOf(item.Id)].CharacterId;
                                output.push(item);
                            }
                        });
                        $scope.costumes = $scope.filtered_costumes;
                        $scope.allCostumes = output;
                    },
                        function error() {

                        });
                }

                $scope.costumeBtn = true;
                $scope.createCostume = (string) => {

                    console.log("costume");

                    var costume = {
                        //"Id":null,
                        "Name": string
                        //"ExtraId": null,
                        //"CharacterId":null
                    }

                    costume.ProjectId = parseInt(projectId);
                    costume.sceneId = parseInt($stateParams.sceneId);
                    $http.post(root + 'api/ScenesandScript/CreateCostume', costume).then(function success(response) {
                        $scope.costumeBtn = true;
                        $scope.getscenes();

                    }, function error() { });
                }
                $scope.add_costumes = (id, type, typeId) => {
                    var costume = {
                        "Id": parseInt(id),

                        //"ExtraId": null,
                        //"CharacterId":null
                    }
                    if (type == 0) {
                        costume.CharacterId = parseInt(typeId);
                    } else {
                        costume.ExtraId = parseInt(typeId);
                    }
                    costume.projectId = parseInt(projectId);
                    costume.sceneId = parseInt($stateParams.sceneId);
                    $http.post(root + 'api/ScenesandScript/CreateCostume', costume).then(function success(response) {
                        $scope.costumeBtn = true;
                        $scope.getscenes();

                    }, function error() { });
                }
                $scope.complete_costume = function (e, string) {
                    //$scope.filtered_pages = null;
                    if (string == null || string == "undefined") {

                        $scope.costumeBtn = true;
                        $scope.filtered_costumes = $scope.costumes;
                    } else if (e.keyCode === 13) {
                        $scope.createCostume(string);
                    } else {

                        var output = [];
                        var chk = false;
                        angular.forEach($scope.costumes, function (item) {

                            if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                output.push(item);
                                var exist = $scope.EditScene.CharacterId.includes(item.Id.toString());

                                if (exist) {
                                    chk = true;

                                }
                            }
                        });
                        if (output.length > 0 && !chk) {
                            $scope.costumeBtn = true;
                            $scope.filtered_costumes = output;
                        } else if (output.length == 0) {
                            $scope.filtered_costumes = output;
                            $scope.costumeBtn = false;
                        }
                        else if (chk) {
                            $scope.costumeBtn = true;
                        }
                        else {
                            $scope.costumeBtn = false;
                        }

                    }
                }

                //MakeUp


                $scope.MakeupIndex = -1;
                $scope.show_makeup = function (index, char) {
                    $scope.MakeupIndex = index + char;
                }
                $scope.hide_makeup = function () {
                    $scope.MakeupIndex = -1;
                }

                $scope.checkMakeup = (id, type) => {
                    if ($scope.allMakeup) {
                        if (type == 0) {
                            var arrayWithIds = $scope.allMakeup.map(function (x) {
                                return x.CharacterId;
                            });
                            var present = arrayWithIds.indexOf(id) != -1
                            //console.log(present, id, type);
                            return present

                        } else {
                            var arrayWithIds = $scope.allMakeup.map(function (x) {
                                return x.ExtraId;
                            });
                            var present = arrayWithIds.indexOf(id) != -1
                            //console.log(present, id, type);
                            return present

                            //$scope.Costume = null;

                        }
                    }
                }
                $scope.removeMakeup = (id, type) => {
                    var makeup = {
                        "Id": parseInt(id),

                        //"ExtraId": null,
                        //"CharacterId":null
                    }
                    if (type == 0) {
                        makeup.CharacterId = 0;
                    } else {
                        makeup.ExtraId = 0;
                    }

                    $http.post(root + 'api/ScenesandScript/RemoveMakeup', makeup).then(function success(response) {
                        $scope.charIndex = -1;
                        $scope.getscenes();

                    }, function error() { });
                }
                var getMakeup = () => {
                    $http.get(root + 'api/ScenesandScript/GetMakeup?projectId=' + projectId).then(function (response) {
                        //console.log(response);
                        $scope.makeup = response.data;
                        $scope.allMakeup = response.data;
                        var arrayWithIds = $scope.EditScene.makeups.map(function (x) {
                            return x.Makeup.Id;
                        });
                        output = [];
                        $scope.filtered_makeup = $scope.makeup.filter(function (item) {
                            var present = arrayWithIds.indexOf(item.Id) != -1
                            //if (!$scope.EditScene.makups[arrayWithIds.indexOf(item.Id)].CharacterId) {
                            //    Cid = "";
                            //} else {
                            //    Cid = $scope.EditScene.makups[arrayWithIds.indexOf(item.Id)].CharacterId;
                            //}
                            //if (!$scope.EditScene.makups[arrayWithIds.indexOf(item.Id)].ExtraId) {
                            //    eid = "";
                            //} else {
                            //eid = $scope.EditScene.makups[arrayWithIds.indexOf(item.Id)].ExtraId;;
                            //}
                            //var cp = $scope.EditScene.CharacterId.indexOf(Cid.toString()) != -1;
                            //var ex = $scope.EditScene.ExtraId.indexOf(eid.toString()) != -1;
                            //console.log(present, id, type);
                            if (!present) {
                                return item;
                            } else {
                                item.ExtraId = $scope.EditScene.makeups[arrayWithIds.indexOf(item.Id)].ExtraId;
                                item.CharacterId = $scope.EditScene.makeups[arrayWithIds.indexOf(item.Id)].CharacterId;
                                output.push(item);
                            }
                        });
                        $scope.makeup = $scope.filtered_makeup;
                        $scope.allMakeup = output;

                    },
                        function error() {

                        });
                }

                $scope.makeupBtn = true;
                $scope.createMakeup = (string) => {
                    var makeup = {
                        //"Id":null,
                        "Name": string
                        //"ExtraId": null,
                        //"CharacterId":null
                    }

                    makeup.projectId = parseInt(projectId);
                    makeup.sceneId = parseInt($stateParams.sceneId);

                    $http.post(root + 'api/ScenesandScript/CreateMakeup', makeup).then(function success(response) {
                        $scope.makeupBtn = true;
                        $scope.getscenes();

                    }, function error() { });
                }
                $scope.add_makeup = (id, type, typeId) => {
                    var makeup = {
                        "Id": parseInt(id),

                        //"ExtraId": null,
                        //"CharacterId":null
                    }
                    if (type == 0) {
                        makeup.CharacterId = parseInt(typeId);
                    } else {
                        makeup.ExtraId = parseInt(typeId);
                    }
                    makeup.projectId = parseInt(projectId);
                    makeup.sceneId = parseInt($stateParams.sceneId);
                    $http.post(root + 'api/ScenesandScript/CreateMakeup', makeup).then(function success(response) {
                        $scope.makeupBtn = true;
                        $scope.getscenes();

                    }, function error() { });
                }
                $scope.complete_makeup = function (e, string) {
                    //$scope.filtered_pages = null;
                    if (string == null || string == "undefined") {

                        $scope.makeupBtn = true;
                        $scope.filtered_makeup = $scope.makeup;
                    } else if (e.keyCode === 13) {
                        $scope.createMakeup(string);
                    } else {

                        var output = [];
                        var chk = false;
                        angular.forEach($scope.makeup, function (item) {

                            if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                output.push(item);
                                var exist = $scope.EditScene.CharacterId.includes(item.Id.toString());

                                if (exist) {
                                    chk = true;

                                }
                            }
                        });
                        if (output.length > 0 && !chk) {
                            $scope.makeupBtn = true;
                            $scope.filtered_makeup = output;
                        } else if (output.length == 0) {
                            $scope.filtered_makeup = output;
                            $scope.makeupBtn = false;
                        }
                        else if (chk) {
                            $scope.makeupBtn = true;
                        }
                        else {
                            $scope.makeupBtn = false;
                        }

                    }
                }
                //Construction
                $scope.constbtn = true;
                $scope.getConstruction = function () {
                    $http.get(root + 'api/ScenesandScript/GetConstruction?projectid=' + projectId).then(function success(response) {

                        console.log(response);
                        $scope.consts = response.data;
                        var arrayWithIds = $scope.EditScene.construction.map(function (x) {
                            return x.Id;
                        });

                        $scope.filtered_const = $scope.consts.filter(function (item) {
                            var present = arrayWithIds.indexOf(item.Id) != -1
                            //console.log(present, id, type);
                            if (!present) {
                                return item;
                            }
                        });
                        console.log($scope.chars);



                    }, function error() { });
                }
                $scope.createconst = (ch) => {
                    var newch = {
                        "Name": ch,
                        "ProjectId": parseInt(projectId)
                    }
                    $http.post(root + 'api/ScenesandScript/CreateConstruction', newch).then(
                        function success(resp) {
                            if (resp.status === 200) {
                                $scope.getConstruction();
                                $scope.constbtn = true;
                            }
                        }, function error() { });

                }
                $scope.complete_const = function (e, string) {
                    //$scope.filtered_pages = null;
                    if (string == null || string == "undefined") {
                        $scope.const_to_add = false;
                        $scope.constbtn = true;
                        $scope.filtered_const = $scope.consts;
                    } else if (e.keyCode === 13) {
                        $scope.createconst(string);
                    } else {

                        var output = [];
                        var chk = false;
                        angular.forEach($scope.consts, function (item) {

                            if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                output.push(item);
                                var exist = $scope.EditScene.construction.includes(item.Id.toString());

                                if (exist) {
                                    chk = true;

                                }
                            }
                        });
                        if (output.length > 0 && !chk) {
                            $scope.constbtn = true;
                            $scope.filtered_const = output;
                        } else if (output.length == 0) {
                            $scope.filtered_const = output;
                            $scope.constbtn = false;
                        }
                        else if (chk) {
                            $scope.constbtn = true;
                        }
                        else {
                            $scope.constbtn = false;
                        }

                    }
                }
                $scope.add_const_check = true;
                $scope.const_to_add = true;


                $scope.show_add_const = () => {
                    $scope.const_to_add = true;
                }
                $scope.show_const = () => {
                    $scope.const_to_add = false;
                }
                $scope.add_const = (id) => {
                    //$scope.filtered_consts.forEach(function (item) {
                    //    //console.log(item)
                    //    if (item.Id == id) {

                    //        $scope.addedconst.push(item);
                    //    }
                    //});
                    $scope.filtered_const = $scope.filtered_const.filter(function (item) {
                        if (item.Id != id) {
                            return item;
                        }
                    });

                    //console.log($scope.addedconst);

                    if ($scope.filtered_const.length > 0) {
                        $scope.const_to_add = false;
                    } else {
                        $scope.const_to_add = true;
                    }
                    var sceneconstruction = {
                        "SceneId": parseInt($scope.EditScene.Id),
                        "ConstructionId": id
                    }
                    $http.post(root + 'api/ScenesandScript/AddConstruction/', (sceneconstruction)
                    ).then(response => {
                        if (response.status == 200) {
                            $scope.getscenes();
                        }
                    }, err => {

                    });

                }
                $scope.removeconst = function (id) {
                    var data = {
                        "ConstructionId": id,
                        "SceneId": parseInt($scope.scene_id)

                    }
                    $http.post(root + 'api/ScenesandScript/DeleteConstruction', data).then(function success(response) {
                        console.log(response);
                        $scope.charIndex = -1;
                        $scope.getscenes();


                    }, function error() { });
                }



                //Dressing
                $scope.dressbtn = true;
                $scope.getdressing = function () {
                    $http.get(root + 'api/ScenesandScript/GetDressing?projectid=' + projectId).then(function success(response) {

                        console.log(response);
                        $scope.dresss = response.data;
                        var arrayWithIds = $scope.EditScene.dressing.map(function (x) {
                            return x.Id;
                        });

                        $scope.filtered_dress = $scope.dresss.filter(function (item) {
                            var present = arrayWithIds.indexOf(item.Id) != -1
                            //console.log(present, id, type);
                            if (!present) {
                                return item;
                            }
                        });




                    }, function error() { });
                }
                $scope.createdress = (ch) => {
                    var newch = {
                        "Name": ch,
                        "ProjectId": parseInt(projectId)
                    }
                    $http.post(root + 'api/ScenesandScript/CreateDressing', newch).then(
                        function success(resp) {
                            if (resp.status === 200) {
                                $scope.getdressing();
                                $scope.dressbtn = true;
                            }
                        }, function error() { });

                }
                $scope.complete_dress = function (e, string) {
                    //$scope.filtered_pages = null;
                    if (string == null || string == "undefined") {
                        $scope.dress_to_add = false;
                        $scope.dressbtn = true;
                        $scope.filtered_dress = $scope.dresss;
                    } else if (e.keyCode === 13) {
                        $scope.createdress(string);
                    } else {

                        var output = [];
                        var chk = false;
                        angular.forEach($scope.dresss, function (item) {

                            if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                output.push(item);
                                var exist = $scope.EditScene.dressing.includes(item.Id.toString());

                                if (exist) {
                                    chk = true;

                                }
                            }
                        });
                        if (output.length > 0 && !chk) {
                            $scope.dressbtn = true;
                            $scope.filtered_dress = output;
                        } else if (output.length == 0) {
                            $scope.filtered_dress = output;
                            $scope.dressbtn = false;
                        }
                        else if (chk) {
                            $scope.dressbtn = true;
                        }
                        else {
                            $scope.dressbtn = false;
                        }

                    }
                }
                $scope.add_dress_check = true;
                $scope.dress_to_add = true;


                $scope.show_add_dress = () => {
                    $scope.dress_to_add = true;
                }
                $scope.show_dress = () => {
                    $scope.dress_to_add = false;
                }
                $scope.add_dress = (id) => {
                    //$scope.filtered_dresss.forEach(function (item) {
                    //    //console.log(item)
                    //    if (item.Id == id) {

                    //        $scope.addeddress.push(item);
                    //    }
                    //});
                    $scope.filtered_dress = $scope.filtered_dress.filter(function (item) {
                        if (item.Id != id) {
                            return item;
                        }
                    });

                    //console.log($scope.addeddress);

                    if ($scope.filtered_dress.length > 0) {
                        $scope.dress_to_add = false;
                    } else {
                        $scope.dress_to_add = true;
                    }
                    var scenedressing = {
                        "SceneId": parseInt($scope.EditScene.Id),
                        "DressingId": id
                    }
                    $http.post(root + 'api/ScenesandScript/AddDressing/', (scenedressing)
                    ).then(response => {
                        if (response.status == 200) {
                            $scope.getscenes();
                        }
                    }, err => {

                    });

                }
                $scope.removedress = function (id) {
                    var data = {
                        "DressingId": id,
                        "SceneId": parseInt($scope.scene_id)

                    }
                    $http.post(root + 'api/ScenesandScript/DeleteDressing', data).then(function success(response) {
                        console.log(response);
                        $scope.charIndex = -1;
                        $scope.getscenes();


                    }, function error() { });
                }

                //Props
                $scope.propbtn = true;
                $scope.getProps = function () {
                    $http.get(root + 'api/ScenesandScript/GetProps?projectid=' + projectId).then(function success(response) {

                        console.log(response);
                        $scope.props = response.data;
                        var arrayWithIds = $scope.EditScene.Prop.map(function (x) {
                            return x.Id;
                        });

                        $scope.filtered_prop = $scope.props.filter(function (item) {
                            var present = arrayWithIds.indexOf(item.Id) != -1
                            //console.log(present, id, type);
                            if (!present) {
                                return item;
                            }
                        });




                    }, function error() { });
                }
                $scope.createprop = (ch) => {
                    var newch = {
                        "Name": ch,
                        "ProjectId": parseInt(projectId)
                    }
                    $http.post(root + 'api/ScenesandScript/CreateProps', newch).then(
                        function success(resp) {
                            if (resp.status === 200) {
                                $scope.getProps();
                                $scope.propbtn = true;
                            }
                        }, function error() { });

                }
                $scope.complete_prop = function (e, string) {
                    //$scope.filtered_pages = null;
                    if (string == null || string == "undefined") {
                        $scope.prop_to_add = false;
                        $scope.propbtn = true;
                        $scope.filtered_prop = $scope.props;
                    } else if (e.keyCode === 13) {
                        $scope.createprop(string);
                    } else {

                        var output = [];
                        var chk = false;
                        angular.forEach($scope.props, function (item) {

                            if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                output.push(item);
                                var exist = $scope.EditScen.Prop.includes(item.Id.toString());

                                if (exist) {
                                    chk = true;

                                }
                            }
                        });
                        if (output.length > 0 && !chk) {
                            $scope.propbtn = true;
                            $scope.filtered_prop = output;
                        } else if (output.length == 0) {
                            $scope.filtered_prop = output;
                            $scope.propbtn = false;
                        }
                        else if (chk) {
                            $scope.propbtn = true;
                        }
                        else {
                            $scope.propbtn = false;
                        }

                    }
                }
                $scope.add_prop_check = true;
                $scope.prop_to_add = true;


                $scope.show_add_prop = () => {
                    $scope.prop_to_add = true;
                }
                $scope.show_prop = () => {
                    $scope.prop_to_add = false;
                }
                $scope.add_prop = (id) => {
                    //$scope.filtered_props.forEach(function (item) {
                    //    //console.log(item)
                    //    if (item.Id == id) {

                    //        $scope.addedprop.push(item);
                    //    }
                    //});
                    $scope.filtered_prop = $scope.filtered_prop.filter(function (item) {
                        if (item.Id != id) {
                            return item;
                        }
                    });

                    //console.log($scope.addedprop);

                    if ($scope.filtered_prop.length > 0) {
                        $scope.prop_to_add = false;
                    } else {
                        $scope.prop_to_add = true;
                    }
                    var sceneprops = {
                        "SceneId": parseInt($scope.EditScene.Id),
                        "PropsId": id
                    }
                    $http.post(root + 'api/ScenesandScript/AddProps/', (sceneprops)
                    ).then(response => {
                        if (response.status == 200) {
                            $scope.getscenes();
                        }
                    }, err => {

                    });

                }
                $scope.removeprop = function (id) {
                    var data = {
                        "PropsId": id,
                        "SceneId": parseInt($scope.scene_id)

                    }
                    $http.post(root + 'api/ScenesandScript/DeleteProps', data).then(function success(response) {
                        console.log(response);
                        $scope.charIndex = -1;
                        $scope.getscenes();


                    }, function error() { });
                }


                //Graphic
                $scope.graphicbtn = true;
                $scope.getgraphic = function () {
                    $http.get(root + 'api/ScenesandScript/GetGraphic?projectid=' + projectId).then(function success(response) {

                        console.log(response);
                        $scope.graphics = response.data;
                        var arrayWithIds = $scope.EditScene.graphics.map(function (x) {
                            return x.Id;
                        });

                        $scope.filtered_graphic = $scope.graphics.filter(function (item) {
                            var present = arrayWithIds.indexOf(item.Id) != -1
                            //console.log(present, id, type);
                            if (!present) {
                                return item;
                            }
                        });




                    }, function error() { });
                }
                $scope.creategraphic = (ch) => {
                    var newch = {
                        "Name": ch,
                        "ProjectId": parseInt(projectId)
                    }
                    $http.post(root + 'api/ScenesandScript/CreateGraphic', newch).then(
                        function success(resp) {
                            if (resp.status === 200) {
                                $scope.getgraphic();
                                $scope.graphicbtn = true;
                            }
                        }, function error() { });

                }
                $scope.complete_graphic = function (e, string) {
                    //$scope.filtered_pages = null;
                    if (string == null || string == "undefined") {
                        $scope.graphic_to_add = false;
                        $scope.graphicbtn = true;
                        $scope.filtered_graphic = $scope.Graphic;
                    } else if (e.keyCode === 13) {
                        $scope.creategraphic(string);
                    } else {

                        var output = [];
                        var chk = false;
                        angular.forEach($scope.graphics, function (item) {

                            if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                output.push(item);
                                var exist = $scope.EditScen.graphics.includes(item.Id.toString());

                                if (exist) {
                                    chk = true;

                                }
                            }
                        });
                        if (output.length > 0 && !chk) {
                            $scope.graphicbtn = true;
                            $scope.filtered_graphic = output;
                        } else if (output.length == 0) {
                            $scope.filtered_graphic = output;
                            $scope.graphicbtn = false;
                        }
                        else if (chk) {
                            $scope.graphicbtn = true;
                        }
                        else {
                            $scope.graphicbtn = false;
                        }

                    }
                }
                $scope.add_graphic_check = true;
                $scope.graphic_to_add = true;


                $scope.show_add_graphic = () => {
                    $scope.graphic_to_add = true;
                }
                $scope.show_graphic = () => {
                    $scope.graphic_to_add = false;
                }
                $scope.add_graphic = (id) => {
                    //$scope.filtered_graphics.forEach(function (item) {
                    //    //console.log(item)
                    //    if (item.Id == id) {

                    //        $scope.addedgraphic.push(item);
                    //    }
                    //});
                    $scope.filtered_graphic = $scope.filtered_graphic.filter(function (item) {
                        if (item.Id != id) {
                            return item;
                        }
                    });

                    //console.log($scope.addedgraphic);

                    if ($scope.filtered_graphic.length > 0) {
                        $scope.graphic_to_add = false;
                    } else {
                        $scope.graphic_to_add = true;
                    }
                    var sceneGraphic = {
                        "SceneId": parseInt($scope.EditScene.Id),
                        "GraphicId": id
                    }
                    $http.post(root + 'api/ScenesandScript/AddGraphic/', (sceneGraphic)
                    ).then(response => {
                        if (response.status == 200) {
                            $scope.getscenes();
                        }
                    }, err => {

                    });

                }
                $scope.removegraphic = function (id) {
                    var data = {
                        "GraphicId": id,
                        "SceneId": parseInt($scope.scene_id)

                    }
                    $http.post(root + 'api/ScenesandScript/DeleteGraphic', data).then(function success(response) {
                        console.log(response);
                        $scope.charIndex = -1;
                        $scope.getscenes();


                    }, function error() { });
                }


                //Vehicle
                $scope.vehiclebtn = true;
                $scope.getvehicle = function () {
                    $http.get(root + 'api/ScenesandScript/GetVehicle?projectid=' + projectId).then(function success(response) {

                        console.log(response);
                        $scope.vehicles = response.data;
                        var arrayWithIds = $scope.EditScene.vehicles.map(function (x) {
                            return x.Id;
                        });

                        $scope.filtered_vehicle = $scope.vehicles.filter(function (item) {
                            var present = arrayWithIds.indexOf(item.Id) != -1
                            //console.log(present, id, type);
                            if (!present) {
                                return item;
                            }
                        });




                    }, function error() { });
                }
                $scope.createvehicle = (ch) => {
                    var newch = {
                        "Name": ch,
                        "ProjectId": parseInt(projectId)
                    }
                    $http.post(root + 'api/ScenesandScript/CreateVehicle', newch).then(
                        function success(resp) {
                            if (resp.status === 200) {
                                $scope.getvehicle();
                                $scope.vehiclebtn = true;
                            }
                        }, function error() { });

                }
                $scope.complete_vehicle = function (e, string) {
                    //$scope.filtered_pages = null;
                    if (string == null || string == "undefined") {
                        $scope.vehicle_to_add = false;
                        $scope.vehiclebtn = true;
                        $scope.filtered_vehicle = $scope.vehicle;
                    } else if (e.keyCode === 13) {
                        $scope.createvehicle(string);
                    } else {

                        var output = [];
                        var chk = false;
                        angular.forEach($scope.vehicles, function (item) {

                            if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                output.push(item);
                                var exist = $scope.EditScen.vehicles.includes(item.Id.toString());

                                if (exist) {
                                    chk = true;

                                }
                            }
                        });
                        if (output.length > 0 && !chk) {
                            $scope.vehiclebtn = true;
                            $scope.filtered_vehicle = output;
                        } else if (output.length == 0) {
                            $scope.filtered_vehicle = output;
                            $scope.vehiclebtn = false;
                        }
                        else if (chk) {
                            $scope.vehiclebtn = true;
                        }
                        else {
                            $scope.vehiclebtn = false;
                        }

                    }
                }
                $scope.add_vehicle_check = true;
                $scope.vehicle_to_add = true;


                $scope.show_add_vehicle = () => {
                    $scope.vehicle_to_add = true;
                }
                $scope.show_vehicle = () => {
                    $scope.vehicle_to_add = false;
                }
                $scope.add_vehicle = (id) => {
                    //$scope.filtered_vehicles.forEach(function (item) {
                    //    //console.log(item)
                    //    if (item.Id == id) {

                    //        $scope.addedvehicle.push(item);
                    //    }
                    //});
                    $scope.filtered_vehicle = $scope.filtered_vehicle.filter(function (item) {
                        if (item.Id != id) {
                            return item;
                        }
                    });

                    //console.log($scope.addedvehicle);

                    if ($scope.filtered_vehicle.length > 0) {
                        $scope.vehicle_to_add = false;
                    } else {
                        $scope.vehicle_to_add = true;
                    }
                    var scenevehicle = {
                        "SceneId": parseInt($scope.EditScene.Id),
                        "VehicleId": id
                    }
                    $http.post(root + 'api/ScenesandScript/AddVehicle/', (scenevehicle)
                    ).then(response => {
                        if (response.status == 200) {
                            $scope.getscenes();
                        }
                    }, err => {

                    });

                }
                $scope.removevehicle = function (id) {
                    var data = {
                        "VehicleId": id,
                        "SceneId": parseInt($scope.scene_id)

                    }
                    $http.post(root + 'api/ScenesandScript/DeleteVehicle', data).then(function success(response) {
                        console.log(response);
                        $scope.charIndex = -1;
                        $scope.getscenes();


                    }, function error() { });
                }


                //Animal
                $scope.animalbtn = true;
                $scope.getanimal = function () {
                    $http.get(root + 'api/ScenesandScript/GetAnimal?projectid=' + projectId).then(function success(response) {

                        console.log(response);
                        $scope.animals = response.data;
                        var arrayWithIds = $scope.EditScene.animals.map(function (x) {
                            return x.Id;
                        });

                        $scope.filtered_animal = $scope.animals.filter(function (item) {
                            var present = arrayWithIds.indexOf(item.Id) != -1
                            //console.log(present, id, type);
                            if (!present) {
                                return item;
                            }
                        });




                    }, function error() { });
                }
                $scope.createanimal = (ch) => {
                    var newch = {
                        "Name": ch,
                        "ProjectId": parseInt(projectId)
                    }
                    $http.post(root + 'api/ScenesandScript/CreateAnimal', newch).then(
                        function success(resp) {
                            if (resp.status === 200) {
                                $scope.getanimal();
                                $scope.animalbtn = true;
                            }
                        }, function error() { });

                }
                $scope.complete_animal = function (e, string) {
                    //$scope.filtered_pages = null;
                    if (string == null || string == "undefined") {
                        $scope.animal_to_add = false;
                        $scope.animalbtn = true;
                        $scope.filtered_animal = $scope.animal;
                    } else if (e.keyCode === 13) {
                        $scope.createanimal(string);
                    } else {

                        var output = [];
                        var chk = false;
                        angular.forEach($scope.animals, function (item) {

                            if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                output.push(item);
                                var exist = $scope.EditScen.animals.includes(item.Id.toString());

                                if (exist) {
                                    chk = true;

                                }
                            }
                        });
                        if (output.length > 0 && !chk) {
                            $scope.animalbtn = true;
                            $scope.filtered_animal = output;
                        } else if (output.length == 0) {
                            $scope.filtered_animal = output;
                            $scope.animalbtn = false;
                        }
                        else if (chk) {
                            $scope.animalbtn = true;
                        }
                        else {
                            $scope.animalbtn = false;
                        }

                    }
                }
                $scope.add_animal_check = true;
                $scope.animal_to_add = true;


                $scope.show_add_animal = () => {
                    $scope.animal_to_add = true;
                }
                $scope.show_animal = () => {
                    $scope.animal_to_add = false;
                }
                $scope.add_animal = (id) => {
                    //$scope.filtered_animals.forEach(function (item) {
                    //    //console.log(item)
                    //    if (item.Id == id) {

                    //        $scope.addedanimal.push(item);
                    //    }
                    //});
                    $scope.filtered_animal = $scope.filtered_animal.filter(function (item) {
                        if (item.Id != id) {
                            return item;
                        }
                    });

                    //console.log($scope.addedanimal);

                    if ($scope.filtered_animal.length > 0) {
                        $scope.animal_to_add = false;
                    } else {
                        $scope.animal_to_add = true;
                    }
                    var sceneanimal = {
                        "SceneId": parseInt($scope.EditScene.Id),
                        "AnimalId": id
                    }
                    $http.post(root + 'api/ScenesandScript/AddAnimal/', (sceneanimal)
                    ).then(response => {
                        if (response.status == 200) {
                            $scope.getscenes();
                        }
                    }, err => {

                    });

                }
                $scope.removeanimal = function (id) {
                    var data = {
                        "AnimalId": id,
                        "SceneId": parseInt($scope.scene_id)

                    }
                    $http.post(root + 'api/ScenesandScript/DeleteAnimal', data).then(function success(response) {
                        console.log(response);
                        $scope.charIndex = -1;
                        $scope.getscenes();


                    }, function error() { });
                }
                //VisualEffects
                $scope.visualbtn = true;
                $scope.getvisual = function () {
                    $http.get(root + 'api/ScenesandScript/GetVisual?projectid=' + projectId).then(function success(response) {

                        console.log(response);
                        $scope.visuals = response.data;
                        var arrayWithIds = $scope.EditScene.visuals.map(function (x) {
                            return x.Id;
                        });

                        $scope.filtered_visual = $scope.visuals.filter(function (item) {
                            var present = arrayWithIds.indexOf(item.Id) != -1
                            //console.log(present, id, type);
                            if (!present) {
                                return item;
                            }
                        });




                    }, function error() { });
                }
                $scope.createvisual = (ch) => {
                    var newch = {
                        "Name": ch,
                        "ProjectId": parseInt(projectId)
                    }
                    $http.post(root + 'api/ScenesandScript/CreateVisual', newch).then(
                        function success(resp) {
                            if (resp.status === 200) {
                                $scope.getvisual();
                                $scope.visualbtn = true;
                            }
                        }, function error() { });

                }
                $scope.complete_visual = function (e, string) {
                    //$scope.filtered_pages = null;
                    if (string == null || string == "undefined") {
                        $scope.visual_to_add = false;
                        $scope.visualbtn = true;
                        $scope.filtered_visual = $scope.visual;
                    } else if (e.keyCode === 13) {
                        $scope.createvisual(string);
                    } else {

                        var output = [];
                        var chk = false;
                        angular.forEach($scope.visuals, function (item) {

                            if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                output.push(item);
                                var exist = $scope.EditScen.visuals.includes(item.Id.toString());

                                if (exist) {
                                    chk = true;

                                }
                            }
                        });
                        if (output.length > 0 && !chk) {
                            $scope.visualbtn = true;
                            $scope.filtered_visual = output;
                        } else if (output.length == 0) {
                            $scope.filtered_visual = output;
                            $scope.visualbtn = false;
                        }
                        else if (chk) {
                            $scope.visualbtn = true;
                        }
                        else {
                            $scope.visualbtn = false;
                        }

                    }
                }
                $scope.add_visual_check = true;
                $scope.visual_to_add = true;


                $scope.show_add_visual = () => {
                    $scope.visual_to_add = true;
                }
                $scope.show_visual = () => {
                    $scope.visual_to_add = false;
                }
                $scope.add_visual = (id) => {
                    //$scope.filtered_visuals.forEach(function (item) {
                    //    //console.log(item)
                    //    if (item.Id == id) {

                    //        $scope.addedvisual.push(item);
                    //    }
                    //});
                    $scope.filtered_visual = $scope.filtered_visual.filter(function (item) {
                        if (item.Id != id) {
                            return item;
                        }
                    });

                    //console.log($scope.addedvisual);

                    if ($scope.filtered_visual.length > 0) {
                        $scope.visual_to_add = false;
                    } else {
                        $scope.visual_to_add = true;
                    }
                    var scenevisual = {
                        "SceneId": parseInt($scope.EditScene.Id),
                        "VisualId": id
                    }
                    $http.post(root + 'api/ScenesandScript/AddVisual/', (scenevisual)
                    ).then(response => {
                        if (response.status == 200) {
                            $scope.getscenes();
                        }
                    }, err => {

                    });

                }
                $scope.removevisual = function (id) {
                    var data = {
                        "VisualId": id,
                        "SceneId": parseInt($scope.scene_id)

                    }
                    $http.post(root + 'api/ScenesandScript/DeleteVisual', data).then(function success(response) {
                        console.log(response);
                        $scope.charIndex = -1;
                        $scope.getscenes();


                    }, function error() { });
                }


                //SpecialEffects
                $scope.specialbtn = true;
                $scope.getspecial = function () {
                    $http.get(root + 'api/ScenesandScript/GetSpecial?projectid=' + projectId).then(function success(response) {

                        console.log(response);
                        $scope.specials = response.data;
                        var arrayWithIds = $scope.EditScene.specials.map(function (x) {
                            return x.Id;
                        });

                        $scope.filtered_special = $scope.specials.filter(function (item) {
                            var present = arrayWithIds.indexOf(item.Id) != -1
                            //console.log(present, id, type);
                            if (!present) {
                                return item;
                            }
                        });




                    }, function error() { });
                }
                $scope.createspecial = (ch) => {
                    var newch = {
                        "Name": ch,
                        "ProjectId": parseInt(projectId)
                    }
                    $http.post(root + 'api/ScenesandScript/CreateSpecial', newch).then(
                        function success(resp) {
                            if (resp.status === 200) {
                                $scope.getspecial();
                                $scope.specialbtn = true;
                            }
                        }, function error() { });

                }
                $scope.complete_special = function (e, string) {
                    //$scope.filtered_pages = null;
                    if (string == null || string == "undefined") {
                        $scope.special_to_add = false;
                        $scope.specialbtn = true;
                        $scope.filtered_special = $scope.special;
                    } else if (e.keyCode === 13) {
                        $scope.createspecial(string);
                    } else {

                        var output = [];
                        var chk = false;
                        angular.forEach($scope.specials, function (item) {

                            if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                output.push(item);
                                var exist = $scope.EditScen.specials.includes(item.Id.toString());

                                if (exist) {
                                    chk = true;

                                }
                            }
                        });
                        if (output.length > 0 && !chk) {
                            $scope.specialbtn = true;
                            $scope.filtered_special = output;
                        } else if (output.length == 0) {
                            $scope.filtered_special = output;
                            $scope.specialbtn = false;
                        }
                        else if (chk) {
                            $scope.specialbtn = true;
                        }
                        else {
                            $scope.specialbtn = false;
                        }

                    }
                }
                $scope.add_special_check = true;
                $scope.special_to_add = true;


                $scope.show_add_special = () => {
                    $scope.special_to_add = true;
                }
                $scope.show_special = () => {
                    $scope.special_to_add = false;
                }
                $scope.add_special = (id) => {
                    //$scope.filtered_specials.forEach(function (item) {
                    //    //console.log(item)
                    //    if (item.Id == id) {

                    //        $scope.addedspecial.push(item);
                    //    }
                    //});
                    $scope.filtered_special = $scope.filtered_special.filter(function (item) {
                        if (item.Id != id) {
                            return item;
                        }
                    });

                    //console.log($scope.addedspecial);

                    if ($scope.filtered_special.length > 0) {
                        $scope.special_to_add = false;
                    } else {
                        $scope.special_to_add = true;
                    }
                    var scenespecial = {
                        "SceneId": parseInt($scope.EditScene.Id),
                        "SpecialId": id
                    }
                    $http.post(root + 'api/ScenesandScript/AddSpecial/', (scenespecial)
                    ).then(response => {
                        if (response.status == 200) {
                            $scope.getscenes();
                        }
                    }, err => {

                    });

                }
                $scope.removespecial = function (id) {
                    var data = {
                        "SpecialId": id,
                        "SceneId": parseInt($scope.scene_id)

                    }
                    $http.post(root + 'api/ScenesandScript/DeleteSpecial', data).then(function success(response) {
                        console.log(response);
                        $scope.charIndex = -1;
                        $scope.getscenes();


                    }, function error() { });
                }




                //Sounds
                $scope.soundbtn = true;
                $scope.getsound = function () {
                    $http.get(root + 'api/ScenesandScript/GetSound?projectid=' + projectId).then(function success(response) {

                        console.log(response);
                        $scope.sounds = response.data;
                        var arrayWithIds = $scope.EditScene.sounds.map(function (x) {
                            return x.Id;
                        });

                        $scope.filtered_sound = $scope.sounds.filter(function (item) {
                            var present = arrayWithIds.indexOf(item.Id) != -1
                            //console.log(present, id, type);
                            if (!present) {
                                return item;
                            }
                        });




                    }, function error() { });
                }
                $scope.createsound = (ch) => {
                    var newch = {
                        "Name": ch,
                        "ProjectId": parseInt(projectId)
                    }
                    $http.post(root + 'api/ScenesandScript/CreateSound', newch).then(
                        function success(resp) {
                            if (resp.status === 200) {
                                $scope.getsound();
                                $scope.soundbtn = true;
                            }
                        }, function error() { });

                }
                $scope.complete_sound = function (e, string) {
                    //$scope.filtered_pages = null;
                    if (string == null || string == "undefined") {
                        $scope.sound_to_add = false;
                        $scope.soundbtn = true;
                        $scope.filtered_sound = $scope.sound;
                    } else if (e.keyCode === 13) {
                        $scope.createsound(string);
                    } else {

                        var output = [];
                        var chk = false;
                        angular.forEach($scope.sounds, function (item) {

                            if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                output.push(item);
                                var exist = $scope.EditScen.sounds.includes(item.Id.toString());

                                if (exist) {
                                    chk = true;

                                }
                            }
                        });
                        if (output.length > 0 && !chk) {
                            $scope.soundbtn = true;
                            $scope.filtered_sound = output;
                        } else if (output.length == 0) {
                            $scope.filtered_sound = output;
                            $scope.soundbtn = false;
                        }
                        else if (chk) {
                            $scope.soundbtn = true;
                        }
                        else {
                            $scope.soundbtn = false;
                        }

                    }
                }
                $scope.add_sound_check = true;
                $scope.sound_to_add = true;


                $scope.show_add_sound = () => {
                    $scope.sound_to_add = true;
                }
                $scope.show_sound = () => {
                    $scope.sound_to_add = false;
                }
                $scope.add_sound = (id) => {
                    //$scope.filtered_sounds.forEach(function (item) {
                    //    //console.log(item)
                    //    if (item.Id == id) {

                    //        $scope.addedsound.push(item);
                    //    }
                    //});
                    $scope.filtered_sound = $scope.filtered_sound.filter(function (item) {
                        if (item.Id != id) {
                            return item;
                        }
                    });

                    //console.log($scope.addedsound);

                    if ($scope.filtered_sound.length > 0) {
                        $scope.sound_to_add = false;
                    } else {
                        $scope.sound_to_add = true;
                    }
                    var scenesound = {
                        "SceneId": parseInt($scope.EditScene.Id),
                        "SoundId": id
                    }
                    $http.post(root + 'api/ScenesandScript/AddSound/', (scenesound)
                    ).then(response => {
                        if (response.status == 200) {
                            $scope.getscenes();
                        }
                    }, err => {

                    });

                }
                $scope.removesound = function (id) {
                    var data = {
                        "SoundId": id,
                        "SceneId": parseInt($scope.scene_id)

                    }
                    $http.post(root + 'api/ScenesandScript/DeleteSound', data).then(function success(response) {
                        console.log(response);
                        $scope.charIndex = -1;
                        $scope.getscenes();


                    }, function error() { });
                }




                //Cameras
                $scope.camerabtn = true;
                $scope.getcamera = function () {
                    $http.get(root + 'api/ScenesandScript/GetCamera?projectid=' + projectId).then(function success(response) {

                        console.log(response);
                        $scope.cameras = response.data;
                        var arrayWithIds = $scope.EditScene.cameras.map(function (x) {
                            return x.Id;
                        });

                        $scope.filtered_camera = $scope.cameras.filter(function (item) {
                            var present = arrayWithIds.indexOf(item.Id) != -1
                            //console.log(present, id, type);
                            if (!present) {
                                return item;
                            }
                        });




                    }, function error() { });
                }
                $scope.createcamera = (ch) => {
                    var newch = {
                        "Name": ch,
                        "ProjectId": parseInt(projectId)
                    }
                    $http.post(root + 'api/ScenesandScript/CreateCamera', newch).then(
                        function success(resp) {
                            if (resp.status === 200) {
                                $scope.getcamera();
                                $scope.camerabtn = true;
                            }
                        }, function error() { });

                }
                $scope.complete_camera = function (e, string) {
                    //$scope.filtered_pages = null;
                    if (string == null || string == "undefined") {
                        $scope.camera_to_add = false;
                        $scope.camerabtn = true;
                        $scope.filtered_camera = $scope.camera;
                    } else if (e.keyCode === 13) {
                        $scope.createcamera(string);
                    } else {

                        var output = [];
                        var chk = false;
                        angular.forEach($scope.cameras, function (item) {

                            if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                output.push(item);
                                var exist = $scope.EditScen.cameras.includes(item.Id.toString());

                                if (exist) {
                                    chk = true;

                                }
                            }
                        });
                        if (output.length > 0 && !chk) {
                            $scope.camerabtn = true;
                            $scope.filtered_camera = output;
                        } else if (output.length == 0) {
                            $scope.filtered_camera = output;
                            $scope.camerabtn = false;
                        }
                        else if (chk) {
                            $scope.camerabtn = true;
                        }
                        else {
                            $scope.camerabtn = false;
                        }

                    }
                }
                $scope.add_camera_check = true;
                $scope.camera_to_add = true;


                $scope.show_add_camera = () => {
                    $scope.camera_to_add = true;
                }
                $scope.show_camera = () => {
                    $scope.camera_to_add = false;
                }
                $scope.add_camera = (id) => {
                    //$scope.filtered_cameras.forEach(function (item) {
                    //    //console.log(item)
                    //    if (item.Id == id) {

                    //        $scope.addedcamera.push(item);
                    //    }
                    //});
                    $scope.filtered_camera = $scope.filtered_camera.filter(function (item) {
                        if (item.Id != id) {
                            return item;
                        }
                    });

                    //console.log($scope.addedcamera);

                    if ($scope.filtered_camera.length > 0) {
                        $scope.camera_to_add = false;
                    } else {
                        $scope.camera_to_add = true;
                    }
                    var scenecamera = {
                        "SceneId": parseInt($scope.EditScene.Id),
                        "CameraId": id
                    }
                    $http.post(root + 'api/ScenesandScript/AddCamera/', (scenecamera)
                    ).then(response => {
                        if (response.status == 200) {
                            $scope.getscenes();
                        }
                    }, err => {

                    });

                }
                $scope.removecamera = function (id) {
                    var data = {
                        "CameraId": id,
                        "SceneId": parseInt($scope.scene_id)

                    }
                    $http.post(root + 'api/ScenesandScript/DeleteCamera', data).then(function success(response) {
                        console.log(response);
                        $scope.charIndex = -1;
                        $scope.getscenes();


                    }, function error() { });
                }



                //Stunts
                $scope.stuntbtn = true;
                $scope.getstunt = function () {
                    $http.get(root + 'api/ScenesandScript/GetStunt?projectid=' + projectId).then(function success(response) {

                        console.log(response);
                        $scope.stunts = response.data;
                        var arrayWithIds = $scope.EditScene.stunts.map(function (x) {
                            return x.Id;
                        });

                        $scope.filtered_stunt = $scope.stunts.filter(function (item) {
                            var present = arrayWithIds.indexOf(item.Id) != -1
                            //console.log(present, id, type);
                            if (!present) {
                                return item;
                            }
                        });




                    }, function error() { });
                }
                $scope.createstunt = (ch) => {
                    var newch = {
                        "Name": ch,
                        "ProjectId": parseInt(projectId)
                    }
                    $http.post(root + 'api/ScenesandScript/CreateStunt', newch).then(
                        function success(resp) {
                            if (resp.status === 200) {
                                $scope.getstunt();
                                $scope.stuntbtn = true;
                            }
                        }, function error() { });

                }
                $scope.complete_stunt = function (e, string) {
                    //$scope.filtered_pages = null;
                    if (string == null || string == "undefined") {
                        $scope.stunt_to_add = false;
                        $scope.stuntbtn = true;
                        $scope.filtered_stunt = $scope.stunt;
                    } else if (e.keyCode === 13) {
                        $scope.createstunt(string);
                    } else {

                        var output = [];
                        var chk = false;
                        angular.forEach($scope.stunts, function (item) {

                            if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                output.push(item);
                                var exist = $scope.EditScen.stunts.includes(item.Id.toString());

                                if (exist) {
                                    chk = true;

                                }
                            }
                        });
                        if (output.length > 0 && !chk) {
                            $scope.stuntbtn = true;
                            $scope.filtered_stunt = output;
                        } else if (output.length == 0) {
                            $scope.filtered_stunt = output;
                            $scope.stuntbtn = false;
                        }
                        else if (chk) {
                            $scope.stuntbtn = true;
                        }
                        else {
                            $scope.stuntbtn = false;
                        }

                    }
                }
                $scope.add_stunt_check = true;
                $scope.stunt_to_add = true;


                $scope.show_add_stunt = () => {
                    $scope.stunt_to_add = true;
                }
                $scope.show_stunt = () => {
                    $scope.stunt_to_add = false;
                }
                $scope.add_stunt = (id) => {
                    //$scope.filtered_stunts.forEach(function (item) {
                    //    //console.log(item)
                    //    if (item.Id == id) {

                    //        $scope.addedstunt.push(item);
                    //    }
                    //});
                    $scope.filtered_stunt = $scope.filtered_stunt.filter(function (item) {
                        if (item.Id != id) {
                            return item;
                        }
                    });

                    //console.log($scope.addedstunt);

                    if ($scope.filtered_stunt.length > 0) {
                        $scope.stunt_to_add = false;
                    } else {
                        $scope.stunt_to_add = true;
                    }
                    var scenestunt = {
                        "SceneId": parseInt($scope.EditScene.Id),
                        "StuntId": id
                    }
                    $http.post(root + 'api/ScenesandScript/AddStunt/', (scenestunt)
                    ).then(response => {
                        if (response.status == 200) {
                            $scope.getscenes();
                        }
                    }, err => {

                    });

                }
                $scope.removestunt = function (id) {
                    var data = {
                        "StuntId": id,
                        "SceneId": parseInt($scope.scene_id)

                    }
                    $http.post(root + 'api/ScenesandScript/DeleteStunt', data).then(function success(response) {
                        console.log(response);
                        $scope.charIndex = -1;
                        $scope.getscenes();


                    }, function error() { });
                }



                //Others
                $scope.otherbtn = true;
                $scope.getother = function () {
                    $http.get(root + 'api/ScenesandScript/GetOther?projectid=' + projectId).then(function success(response) {

                        console.log(response);
                        $scope.others = response.data;
                        var arrayWithIds = $scope.EditScene.others.map(function (x) {
                            return x.Id;
                        });

                        $scope.filtered_other = $scope.others.filter(function (item) {
                            var present = arrayWithIds.indexOf(item.Id) != -1
                            //console.log(present, id, type);
                            if (!present) {
                                return item;
                            }
                        });




                    }, function error() { });
                }
                $scope.createother = (ch) => {
                    var newch = {
                        "Name": ch,
                        "ProjectId": parseInt(projectId)
                    }
                    $http.post(root + 'api/ScenesandScript/CreateOther', newch).then(
                        function success(resp) {
                            if (resp.status === 200) {
                                $scope.getother();
                                $scope.otherbtn = true;
                            }
                        }, function error() { });

                }
                $scope.complete_other = function (e, string) {
                    //$scope.filtered_pages = null;
                    if (string == null || string == "undefined") {
                        $scope.other_to_add = false;
                        $scope.otherbtn = true;
                        $scope.filtered_other = $scope.other;
                    } else if (e.keyCode === 13) {
                        $scope.createother(string);
                    } else {

                        var output = [];
                        var chk = false;
                        angular.forEach($scope.others, function (item) {

                            if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                output.push(item);
                                var exist = $scope.EditScen.others.includes(item.Id.toString());

                                if (exist) {
                                    chk = true;

                                }
                            }
                        });
                        if (output.length > 0 && !chk) {
                            $scope.otherbtn = true;
                            $scope.filtered_other = output;
                        } else if (output.length == 0) {
                            $scope.filtered_other = output;
                            $scope.otherbtn = false;
                        }
                        else if (chk) {
                            $scope.otherbtn = true;
                        }
                        else {
                            $scope.otherbtn = false;
                        }

                    }
                }
                $scope.add_other_check = true;
                $scope.other_to_add = true;


                $scope.show_add_other = () => {
                    $scope.other_to_add = true;
                }
                $scope.show_other = () => {
                    $scope.other_to_add = false;
                }
                $scope.add_other = (id) => {
                    //$scope.filtered_others.forEach(function (item) {
                    //    //console.log(item)
                    //    if (item.Id == id) {

                    //        $scope.addedother.push(item);
                    //    }
                    //});
                    $scope.filtered_other = $scope.filtered_other.filter(function (item) {
                        if (item.Id != id) {
                            return item;
                        }
                    });

                    //console.log($scope.addedother);

                    if ($scope.filtered_other.length > 0) {
                        $scope.other_to_add = false;
                    } else {
                        $scope.other_to_add = true;
                    }
                    var sceneother = {
                        "SceneId": parseInt($scope.EditScene.Id),
                        "OtherId": id
                    }
                    $http.post(root + 'api/ScenesandScript/AddOther/', (sceneother)
                    ).then(response => {
                        if (response.status == 200) {
                            $scope.getscenes();
                        }
                    }, err => {

                    });

                }
                $scope.removeother = function (id) {
                    var data = {
                        "OtherId": id,
                        "SceneId": parseInt($scope.scene_id)

                    }
                    $http.post(root + 'api/ScenesandScript/DeleteOther', data).then(function success(response) {
                        console.log(response);

                        $scope.getscenes();


                    }, function error() { });
                }



                //Files
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
              



                //files and images tab
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
                            //$scope.files.push(fileObj);
                        });
                        if ($scope.uploadedFiles.length > 0) {
                            $scope.$apply(function () {
                                $scope.hasFilesinUppy = true;
                            });

                        }
                        if ($scope.uploadedFiles) {
                            if ($scope.uploadedFiles.length > 0) {


                                angular.forEach($scope.uploadedFiles, function (item) {

                                    item.SceneId = parseInt($scope.scene_id);

                                });
                                $http.post(root + 'api/DocumentFiles/PostDocumentFiles', $scope.uploadedFiles).then(
                                    function success(resp) {
                                        console.log(resp.data);
                                        if (resp.data.length > 0) {

                                            $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount + $scope.uploadedFiles.length;
                                            resp.data.forEach((file) => {
                                                $scope.files.push(file);
                                                $scope.getscenes();
                                                if ($scope.isItDocDacThumbnail)
                                                    $scope.defaultImage = file.FileId;
                                            });
                                            $scope.uppy.reset();
                                        }
                                    }
                                    , function error() { });
                            }
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
                    size: 12
                }
                $scope.getSceneFiles = function () {
                    $http.get(root + 'api/ScenesandScript/GetSceneFiles/' + parseInt($scope.scene_id) + '/' + $scope.filesPaging.page + '/' + $scope.filesPaging.size).then(resp => {
                        $scope.defaultImage = resp.data.FileId;
                        $scope.files = resp.data.list;
                    }, err => {
                    });
                }

                $scope.deleteFile = function ($event, file) {
                    $event.stopPropagation();
                    var index = $scope.files.indexOf(file);
                    if (index > -1)
                        $scope.files.splice(index, 1);

                    $http.post(root + 'api/DocumentFiles/DeleteDocumentFiles/' + file.Id).then(resp => {
                        //$scope.getLinks();
                        $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount - 1;
                        if ($scope.tabContentLength.filesCount < 0) {
                            $scope.tabContentLength.filesCount = 0;
                        }
                    }, err => {
                    });

                }

                //Tasks

                var initializeTaskObj = function () {
                    $scope.taskObj = {
                        AssignedTo: [],
                        Description: "",
                        Title: "",
                        HasDeadline: false,
                        LinkedToSection: false,
                        DocumentCategoryId: null,
                        Section: null,
                        Deadline: "",
                        Id: 0,
                        ProjectId: parseInt($stateParams.id),
                        Section: "",
                        HasSection: false,
                        SectionUrl: "",
                        Section: "",
                        SceneId: parseInt($scope.scene_id)
                    }




                    $http.get(root + 'api/Comments/GetProjectUsersAndGroups/' + $stateParams.id).then(resp => {
                        $scope.projectUsersAndGroups = resp.data;
                    }, err => {
                    });

                    $scope.groupUsers = function (item) {
                        return item.type == 'user' ? 'Users' : (item.type == 'units' ? 'Units' : (item.type == 'groups' ? 'Groups' : 'Users'));
                    }

                    $http.get(root + 'api/Tasks/GetAllTasks/' + $stateParams.id).then(resp => {
                        $scope.mytasks = resp.data;
                        $scope.mytasks = $scope.mytasks.filter(function (item) {


                            if (item.SceneId == parseInt($scope.scene_id)) {
                                //console.log(exist, item.Id);
                                return item;
                            }
                        });
                        $http.get(root + 'api/Documents/GetProjectWiseUserMenu/' + $stateParams.id).then(function success(response) {
                            $scope.sections = response.data.projectUserMenu;
                        }, function error() {
                        });

                        $http.get(root + 'api/Tasks/GetProjectObjects/' + $stateParams.id).then(function success(response) {
                            $scope.objects = response.data;
                        }, function error() {
                        });

                    }, err => {
                    });
                    $scope.createTask = function () {
                        $http.post(root + 'api/ProjectTasks/PostProjectTask/', $scope.taskObj).then(resp => {
                            initializeTaskObj();
                            //$scope.mytasks.push(resp.data);
                        }, err => {
                        });
                    }

                    $scope.goToTaskProfile = function (taskId) {
                        $state.go('tasks.mytasks.taskprofile', { taskId: taskId, id: projectId });
                    }

                    $scope.cancelTaskCreation = function () {
                        if ($scope.taskObj.HasDeadline)
                            $('#toggleCalendarDeadline').trigger('click');

                        if ($scope.taskObj.HasSection)
                            $('#section-selector').removeClass('show');

                        initializeTaskObj();
                    }

                    $scope.changeTaskStatus = function (event, task) {
                        event.stopPropagation();
                        $http.post(root + 'api/ProjectTasks/ChangeProjectTaskStatus/', task).then(resp => {
                            var index = $scope.mytasks.indexOf(task);
                            if (index > -1)
                                $scope.mytasks[index] = resp.data;
                        }, err => {
                        });
                    }

                    $scope.deleteTask = function (event, task) {
                        //event.stopPropagation();
                        $http.post(root + 'api/ProjectTasks/DeleteProjectTask/' + task.Id).then(resp => {
                            var index = $scope.mytasks.indexOf(task);
                            if (index > -1)
                                $scope.mytasks.splice(index, 1);
                        }, err => {
                        });
                    }



                }


                $scope.commentObj = { newComment:false}



                $('textarea.mention').mentionsInput({
                    onDataRequest: function (mode, query, callback) {
                        $http.get(root + 'api/Comments/GetMentions/' + $stateParams.id + '/' + query).then(resp => {
                            responseData = _.filter(resp.data, function (item) { return item.name.toLowerCase().indexOf(query.toLowerCase()) > -1 });
                            callback.call(this, responseData);
                        }, err => {
                            alert('something went wrong');
                        });
                    }
                });

                $scope.sendComment = function () {
                    $('textarea.mention').mentionsInput("val", function (e) {
                        e != "" && $('textarea.mention').mentionsInput("getMentions", function (o) {
                            var res = getAllMatches(/(@)\[(.*?)\]\((.*?):(.*?)\)/g, e);
                            var markup = e;
                            res.forEach(function (item) {
                                e = e.replace(item[0],
                                    item[3] == 'user' ? '<a href="#/professionaldetails/' + item[4] + '" class="font-weight-bold">' + item[2] + '</a>' :

                                        (item[3] == 'units' ? '<a href=#/' + $scope.projectId + '/crew class="font-weight-bold">' + item[2] + '</a>' :

                                            (item[3] == 'groups' ? '<a href=#/' + $scope.projectId + '/crew class="font-weight-bold">' + item[2] + '</a>' : 'Users')))
                            });
                            $scope.commentObj = {
                                Text: e,
                                newComment: false,
                                AnnouncementId: null,
                                MentionUsers: o,
                                Id: 0,
                                MarkupText: markup,
                                SceneId: parseInt($scope.scene_id)
                            };
                            $http.post(root + 'api/Announcements/PostComment', $scope.commentObj).then(resp => {
                               /* $scope.commentObj = resp.data*/;
                                $scope.comments.push(resp.data);

                                //$scope.commentObj.Text = "";
                                $('textarea.mention').mentionsInput("reset");
                                $scope.tabContentLength.commentsCount = $scope.tabContentLength.commentsCount + 1;
                                $scope.commentObj.newComment = false;
                            }, err => {
                            });
                        });
                    });
                }

                $scope.getComments = function () {
                    //GetComments
                    $http.get(root + 'api/ScenesandScript/GetSceneComments/' + parseInt($scope.scene_id)).then(resp => {
                        if (resp.data.length > 0)
                            $scope.commentObj.newComment = false;
                        $scope.comments = resp.data;

                    }, err => {
                    });
                }

                $scope.deleteComment = function (obj) {
                    var index = $scope.comments.indexOf(obj);
                    if (index > -1) {
                        $scope.comments.splice(index, 1);
                    }
                    $scope.tabContentLength.commentsCount = $scope.tabContentLength.commentsCount - 1;
                    if ($scope.tabContentLength.commentsCount < 0) {
                        $scope.tabContentLength.commentsCount = 0;
                        $scope.commentObj.newComment = true;
                    }
                    $http.post(root + 'api/Announcements/DeleteComment/' + obj.Id).then(resp => {

                    }, err => {
                    });
                }

                //UNIT
                $scope.getProjectUnits = function () {
                    $http.get(root + 'api/ProjectUnits/GetProjectUnits?ProjectId=' + projectId).then(function success(response) {
                        $scope.projectUnits = response.data;
                        $scope.unitfiltered_pages = response.data;
                        angular.forEach($scope.projectUnits, function (item) {
                            if (item.Id == $scope.EditScene.Scene.unit) {
                                $scope.unit = item.Name;
                            }
                        });

                        console.log($scope.projectUnits);
                    }, function error() { });
                }

                $scope.createUnit = (string) => {
                    if (string.trim() === '')
                        return false;

                    var projectUnit = {
                        "Name": string,
                        "ProjectId": projectId
                    }

                    $http.post(root + 'api/ProjectUnits/SaveorUpdateProjectUnit', projectUnit).then(function success(response) {
                        if (response.status == 200) {
                            toaster.pop({
                                type: 'success',
                                title: 'Success',
                                body: 'Unit saved successfully!',
                            });
                            $scope.getProjectUnits();
                            $scope.unitlist = false;
                            $scope.unit = "";
                        }
                    }, function error(err) {
                        console.log(err);
                        toaster.pop({
                            type: 'error',
                            title: 'Error',
                            body: err.data,
                        });
                    });
                }

                $scope.unitcomplete = function (e, string) {
                    //$scope.filtered_pages = null;
                    if (string == null || string == "undefined") {
                        $scope.pagelist = true;
                    } else if (e.keyCode == 13) {
                        $scope.createUnit(string);
                    } else {
                        $scope.unitlist = true;
                        var output = [];
                        angular.forEach($scope.projectUnits, function (item) {
                            if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                output.push(item);
                            }
                        });
                        if (output.length > 0) {
                            $scope.unitlist = false;
                        }
                        else {
                            $scope.unitlist = false;
                        }
                        $scope.unitfiltered_pages = output;
                    }
                }

                $scope.unit_view_list = () => {
                    if (unit_list_check === true) {
                        unit_list_check = false;
                        $scope.unitfiltered_pages = null;
                        $scope.unitfiltered_pages = $scope.projectUnits;
                        $scope.unitlist = false;
                    }
                    else {
                        unit_list_check = true;
                        $scope.unitfiltered_pages = null;
                        $scope.unitlist = true;
                    }

                }
                var unitid;
                $scope.unitfillTextbox = function (string, id) {
                    $scope.EditScene.Scene.unit = parseInt(id);
                    $scope.unit = string;

                    $scope.unitfiltered_pages = null;
                    $scope.unitlist = true;
                }
                $scope.nextScene = () => {
                    $state.go($state.current, { sceneId: $scope.next_id })
                }
                $scope.prevScene = () => {
                    $state.go($state.current, { sceneId: $scope.prev_id })
                }
                $scope.deleteScene = (id) => {

                    $rootScope.deleteScene(id)

                    $rootScope.getscenes();
                    $scope.getscenes();
                    var arrayWithIds = $scope.scenes.map(function (x) {
                        return x.Id;
                    });
                    var index = arrayWithIds.indexOf(parseInt($stateParams.sceneId));
                    if (index < $scope.scenes.length - 1 && $scope.next_id > 0 && index > 0) {
                        $scope.nextScene();
                    } else {
                        $scope.prevScene();
                    }


                }
                $scope.getUserProfile = function () {
                    $http.get(root + 'api/UserProfiles/GetCurrentUserProfile').then(function success(response) {
                        console.log(response.data);
                        $scope.userProfile = response.data.UserProfile;


                        $scope.UserPhoto = "data:image/jpg;base64," + $scope.userProfile.Photo;

                        console.log($scope.userProfile);
                    }, function error() { });
                }
                $scope.removeCh = function (id) {
                    var data = {
                        "CharacterId": id,
                        "SceneId": parseInt($scope.scene_id)

                    }
                    $http.post(root + 'api/ScenesandScript/DeleteChar', data).then(function success(response) {
                        console.log(response);
                        $scope.charIndex = -1;
                        $scope.getscenes();


                    }, function error() { });
                }
                $scope.Editinfo = false
                $scope.show_Edit_info = () => {

                    $scope.Editinfo = true;
                }
                $scope.saveGinfo = () => {
                    $scope.Editinfo = false;

                    $scope.EditScene.Scene.environment_id = parseInt($scope.environment.Id);

                    var allscenes = {
                        "ExtraId": null,
                        "CharacterId": null,
                        "Id": null,
                        "scene": $scope.EditScene.Scene


                    }
                    $http.post(root + 'api/ScenesandScript/CreateScene/', (allscenes)
                    ).then(response => {
                        if (response.status == 200) {
                            toaster.pop({
                                type: 'success',
                                title: 'Success',
                                body: 'Infomation Updated successfully!',
                            });
                            $scope.getscenes();
                        }
                    }, err => {
                        toaster.pop({
                            type: 'error',
                            title: 'Error',
                            body: err.data,
                        });
                    })
                }

                $scope.deleteShot = (id) => {
                    $http.post(root + 'api/ScenesandScript/DeleteShot/' + id).then(resp => {
                        $scope.getscenes();
                    }, function error() {


                    });
                }
                $scope.pages = ["0", "1/8", "2/8", "3/8", "4/8", "5/8", "6/8", "7/8", "1",
                    "1 <sup>1<sup>/<sub>8<sub>", "1 <sup>2<sup>/<sub>8<sub>", "1 <sup>3<sup>/<sub>8<sub>", "1 <sup>4<sup>/<sub>8<sub>", "1 <sup>5<sup>/<sub>8<sub>", "1 <sup>6<sup>/<sub>8<sub>", "1 <sup>7<sup>/<sub>8<sub>", "2",
                    "2 <sup>1<sup>/<sub>8<sub>", "2 <sup>2<sup>/<sub>8<sub>", "2 <sup>3<sup>/<sub>8<sub>", "2 <sup>4<sup>/<sub>8<sub>", "2 <sup>5<sup>/<sub>8<sub>", "2 <sup>6<sup>/<sub>8<sub>", "2 <sup>7<sup>/<sub>8<sub>", "3",
                    "3 <sup>1<sup>/<sub>8<sub>", "3 <sup>2<sup>/<sub>8<sub>", "3 <sup>3<sup>/<sub>8<sub>", "3 <sup>4<sup>/<sub>8<sub>", "3 <sup>5<sup>/<sub>8<sub>", "3 <sup>6<sup>/<sub>8<sub>", "3 <sup>7<sup>/<sub>8<sub>", "4",
                    "4 <sup>1<sup>/<sub>8<sub>", "4 <sup>2<sup>/<sub>8<sub>", "4 <sup>3<sup>/<sub>8<sub>", "4 <sup>4<sup>/<sub>8<sub>", "4 <sup>5<sup>/<sub>8<sub>", "4 <sup>6<sup>/<sub>8<sub>", "4 <sup>7<sup>/<sub>8<sub>", "5",
                    "5 <sup>1<sup>/<sub>8<sub>", "5 <sup>2<sup>/<sub>8<sub>", "5 <sup>3<sup>/<sub>8<sub>", "5 <sup>4<sup>/<sub>8<sub>", "5 <sup>5<sup>/<sub>8<sub>", "5 <sup>6<sup>/<sub>8<sub>", "5 <sup>7<sup>/<sub>8<sub>", "6",
                    "6 <sup>1<sup>/<sub>8<sub>", "6 <sup>2<sup>/<sub>8<sub>", "6 <sup>3<sup>/<sub>8<sub>", "6 <sup>4<sup>/<sub>8<sub>", "6 <sup>5<sup>/<sub>8<sub>", "6 <sup>6<sup>/<sub>8<sub>", "6 <sup>7<sup>/<sub>8<sub>", "7",
                    "7 <sup>1<sup>/<sub>8<sub>", "7 <sup>2<sup>/<sub>8<sub>", "7 <sup>3<sup>/<sub>8<sub>", "7 <sup>4<sup>/<sub>8<sub>", "7 <sup>5<sup>/<sub>8<sub>", "7 <sup>6<sup>/<sub>8<sub>", "7 <sup>7<sup>/<sub>8<sub>", "8",
                    "8 <sup>1<sup>/<sub>8<sub>", "8 <sup>2<sup>/<sub>8<sub>", "8 <sup>3<sup>/<sub>8<sub>", "8 <sup>4<sup>/<sub>8<sub>", "8 <sup>5<sup>/<sub>8<sub>", "8 <sup>6<sup>/<sub>8<sub>", "8 <sup>7<sup>/<sub>8<sub>", "9",
                    "9 <sup>1<sup>/<sub>8<sub>", "9 <sup>2<sup>/<sub>8<sub>", "9 <sup>3<sup>/<sub>8<sub>", "9 <sup>4<sup>/<sub>8<sub>", "9 <sup>5<sup>/<sub>8<sub>", "9 <sup>6<sup>/<sub>8<sub>", "9 <sup>7<sup>/<sub>8<sub>", "10",
                    "10 <sup>1<sup>/<sub>8<sub>", "10 <sup>2<sup>/<sub>8<sub>", "10 <sup>3<sup>/<sub>8<sub>", "10 <sup>4<sup>/<sub>8<sub>", "10 <sup>5<sup>/<sub>8<sub>", "10 <sup>6<sup>/<sub>8<sub>", "10 <sup>7<sup>/<sub>8<sub>", "11",
                    "11 <sup>1<sup>/<sub>8<sub>", "11 <sup>2<sup>/<sub>8<sub>", "11 <sup>3<sup>/<sub>8<sub>", "11 <sup>4<sup>/<sub>8<sub>", "11 <sup>5<sup>/<sub>8<sub>", "11 <sup>6<sup>/<sub>8<sub>", "11 <sup>7<sup>/<sub>8<sub>", "12",
                    "12 <sup>1<sup>/<sub>8<sub>", "12 <sup>2<sup>/<sub>8<sub>", "12 <sup>3<sup>/<sub>8<sub>", "12 <sup>4<sup>/<sub>8<sub>", "12 <sup>5<sup>/<sub>8<sub>", "12 <sup>6<sup>/<sub>8<sub>", "12 <sup>7<sup>/<sub>8<sub>", "13",
                    "13 <sup>1<sup>/<sub>8<sub>", "13 <sup>2<sup>/<sub>8<sub>", "13 <sup>3<sup>/<sub>8<sub>", "13 <sup>4<sup>/<sub>8<sub>", "13 <sup>5<sup>/<sub>8<sub>", "13 <sup>6<sup>/<sub>8<sub>", "13 <sup>7<sup>/<sub>8<sub>", "14",
                    "14 <sup>1<sup>/<sub>8<sub>", "14 <sup>2<sup>/<sub>8<sub>", "14 <sup>3<sup>/<sub>8<sub>", "14 <sup>4<sup>/<sub>8<sub>", "14 <sup>5<sup>/<sub>8<sub>", "14 <sup>6<sup>/<sub>8<sub>", "14 <sup>7<sup>/<sub>8<sub>", "15",




                ];
                $scope.Merge = (id, index) => {
                    $scope.MergesceneId = [];
                    $scope.MergesceneId.push(id);
                    $scope.MergeSceneIndex = null;
                    $scope.MergeSceneIndex = index;
                    $scope.filtered_scenes = $scope.scenes.filter(function (item) {
                        if (item.Id != id) {
                            return item;
                        }
                    });
                    $('#myModal').modal('show');
                }
                $scope.EsId = 0;
                $scope.sIndex = -1;

                $scope.show_Es_dropdown = (scene, type) => {
                    if ($scope.EsId === 0) {
                        $scope.Selected = {};
                        $scope.EsId = scene.Id + type;

                        if (type === "eq") {
                            $scope.Selected.Equipment = scene.Equipment;
                        }
                        if (type == "ex") {
                            $scope.getextra();
                        } if (type == "env") {
                            $scope.getenv();
                        } if (type == "set") {
                            $scope.getset();
                        }
                    } else {

                        $scope.EsId = 0;
                        $scope.sIndex = -1;
                        $scope.createShot(scene);
                    }
                }
                $scope.DuplicateShot = (id) => {
                    var shotId = parseInt(id);
                    $http.post(root + "api/ScenesandScript/DuplicateShot/", shotId).then(function (Resp) {
                        if (Resp.status === 200) { $scope.charIndex = -1; $rootScope._getscenes(); }
                    }, function error() { });
                }
                $scope.createShot = (Shot) => {
                    Shot.UnitId = parseInt(Shot.UnitId);
                    Shot.SceneId = parseInt(Shot.SceneId);
                    $http.post(root + "api/ScenesandScript/CreateShot/", Shot).then(function (Resp) {
                        if (Resp.status === 200) {

                            //if ($scope.uploadedFiles) {
                            //    if ($scope.uploadedFiles.length > 0) {

                            //        console.log(Resp);

                            //        angular.forEach($scope.uploadedFiles, function (item) {

                            //            item.ShotId = Resp.data.Id;
                            //            item.SceneId = $scope.Shot.SceneId
                            //        });
                            //        $http.post(root + 'api/DocumentFiles/PostDocumentFiles', $scope.uploadedFiles).then(
                            //            function success(resp) {
                            //                if (resp.data.length > 0) {

                            //                    $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount + $scope.uploadedFiles.length;
                            //                    //resp.data.forEach((file) => {
                            //                    //    $scope.files.push(file);
                            //                    //    if ($scope.isItDocDacThumbnail)
                            //                    //        $scope.defaultImage = file.FileId;
                            //                    //});
                            //                    $scope.uppy.reset();
                            //                }
                            //            }
                            //            , function error() { });
                            //    }
                            //}
                            //$uibModalInstance.dismiss();
                            $rootScope._getscenes();
                        }
                    }, function error() { });
                }
                $scope.size = [
                    { Name: "Close-up", Type: "closeup" },
                    { Name: "Medium Close-up", Type: "closeup" },
                    { Name: "Extreme Close-up", Type: "closeup" },
                    { Name: "Wide Close-up", Type: "closeup" },
                    { Name: "Medium Shot", Type: "medium" },
                    { Name: "Close Shot", Type: "medium" },
                    { Name: "Medium Close Shot", Type: "medium" },
                    { Name: "Wide Shot", Type: "long" },
                    { Name: "Extreme Wide Shot", Type: "long" },
                    { Name: "Full Shot", Type: "long" },
                    { Name: "Medium Full Shot", Type: "long" },
                    { Name: "Long Shot", Type: "long" },
                    { Name: "Extreme Long Shot", Type: "long" },
                ]
                $scope.groupSize = function (item) {
                    //alert(item);
                    return item.Type === 'closeup' ? 'Close-ups' : (item.Type === "medium" ? "Medium Shots" : (item.Type === "long" ? "Long Shots" : ""));
                }

                $scope.type = [
                    { Name: "Eye Level", Type: "height" },
                    { Name: "Low Angle", Type: "height" },
                    { Name: "High Angle", Type: "height" },
                    { Name: "Overhead", Type: "height" },
                    { Name: "Shoulder Level", Type: "height" },
                    { Name: "Hip Level", Type: "height" },
                    { Name: "Knee Level", Type: "height" },
                    { Name: "Ground Level", Type: "height" },
                    { Name: "Single", Type: "framing" },
                    { Name: "Two Shot", Type: "framing" },
                    { Name: "Three Shot", Type: "framing" },
                    { Name: "Over-The-Shoulder", Type: "framing" },
                    { Name: "Over-The-Hip", Type: "framing" },
                    { Name: "Point of View", Type: "framing" },
                    { Name: "Dutch(left)", Type: "Dutch" },
                    { Name: "Dutch(right)", Type: "Dutch" },
                    { Name: "Rack Focus", Type: "focus" },
                    { Name: "Shallow Focus", Type: "focus" },
                    { Name: "Deep Focus", Type: "focus" },
                    { Name: "Tilt-Shift", Type: "focus" },
                    { Name: "Zoom", Type: "focus" },

                ]
                $scope.groupType = function (item) {
                    //alert(item);
                    return item.Type === 'height' ? 'Camera Height' : (item.Type === "framing" ? "Framing" : (item.Type === "Dutch" ? "Dutch Angle" : (item.Type === "focus" ? "Focus / DOF" : "")));
                }

                $scope.movement = [
                    { Name: "Static", Type: "movement" },
                    { Name: "Pan", Type: "movement" },
                    { Name: "Tilt", Type: "movement" },
                    { Name: "Swish Pan", Type: "movement" },
                    { Name: "Swish Tilt", Type: "movement" },
                    { Name: "Tracking", Type: "movement" },


                ]
                $scope.groupMovement = function (item) {
                    //alert(item);
                    return item.Type === 'movement' ? 'Camera Movement' : "";
                }

                $scope.vfx = [
                    { Name: "Motion Graphics", Type: "vfx" },
                    { Name: "Matte Painting", Type: "vfx" },
                    { Name: "Composite", Type: "vfx" }


                ]
                $scope.groupVfx = function (item) {
                    //alert(item);
                    return item.Type === 'vfx' ? 'VFX' : "";
                }
                $scope.camera = [
                    { Name: "CAM A", Type: "cam" },
                    { Name: "CAM B", Type: "cam" },
                    { Name: "CAM C", Type: "cam" },
                    { Name: "CAM D", Type: "cam" },
                    { Name: "CAM E", Type: "cam" },
                    { Name: "CAM F", Type: "cam" },
                    { Name: "CAM G", Type: "cam" },
                    { Name: "CAM H", Type: "cam" },
                    { Name: "CAM I", Type: "cam" }


                ]
                $scope.groupCamera = function (item) {
                    //alert(item);
                    return item.Type === 'cam' ? 'Camera' : "";
                }
                $scope.lens = [
                    { Name: "Normal", Type: "view" },
                    { Name: "Telephoto", Type: "view" },
                    { Name: "Wide Angle", Type: "view" },
                    { Name: "Fish-Eye", Type: "view" },
                    { Name: "Zoom", Type: "view" },
                    { Name: "10mm", Type: "prime" },
                    { Name: "12mm", Type: "prime" },
                    { Name: "14mm", Type: "prime" },
                    { Name: "16mm", Type: "prime" },
                    { Name: "18mm", Type: "prime" },
                    { Name: "20mm", Type: "prime" },
                    { Name: "24mm", Type: "prime" },
                    { Name: "25mm", Type: "prime" },
                    { Name: "28mm", Type: "prime" },
                    { Name: "30mm", Type: "prime" },
                    { Name: "32mm", Type: "prime" },
                    { Name: "35mm", Type: "prime" },
                    { Name: "40mm", Type: "prime" },
                    { Name: "50mm", Type: "prime" },
                    { Name: "65mm", Type: "prime" },
                    { Name: "75mm", Type: "prime" },
                    { Name: "85mm", Type: "prime" },
                    { Name: "100mm", Type: "prime" },
                    { Name: "135mm", Type: "prime" },
                    { Name: "150mm", Type: "prime" },
                    { Name: "180mm", Type: "prime" },


                ]
                $scope.groupLens = function (item) {
                    //alert(item);
                    return item.Type === 'view' ? 'Angle of View' : (item.Type === 'prime' ? 'Primes' : "");
                }
                $scope.fps = [
                    { Name: "24 fps", Type: "fps" },
                    { Name: "23.98 fps", Type: "fps" },
                    { Name: "25 fps", Type: "fps" },
                    { Name: "30 fps", Type: "fps" },
                    { Name: "29.97 fps", Type: "fps" },
                    { Name: "48 fps", Type: "fps" },
                    { Name: "47.95 fps", Type: "fps" },
                    { Name: "50 fps", Type: "fps" },
                    { Name: "59.94 fps", Type: "fps" },
                    { Name: "60 fps", Type: "fps" },
                    { Name: "75 fps", Type: "fps" },
                    { Name: "100 fps", Type: "fps" },
                    { Name: "120 fps", Type: "fps" },
                    { Name: "240 fps", Type: "fps" },
                    { Name: "300 fps", Type: "fps" },

                ]
                $scope.groupFps = function (item) {
                    //alert(item);
                    return item.Type === 'fps' ? 'Frame Rate' : "";
                }
                $scope.specialequipment = [
                    { Name: "GoPro", Type: "se" },
                    { Name: "Drone", Type: "se" },
                    { Name: "High Speed", Type: "se" },
                    { Name: "360°VR", Type: "se" },
                    { Name: "Under-Water", Type: "se" },
                    { Name: "Hand Cracked", Type: "se" },
                    { Name: "3D", Type: "se" },
                    { Name: "IMAX", Type: "se" },


                ]
                $scope.groupSpecialequipment = function (item) {
                    //alert(item);
                    return item.Type === 'se' ? 'Special Equipment' : "";
                }
                $scope.equipment = [
                    { Name: "Sticks", Type: "mck" },
                    { Name: "Hand Held", Type: "mck" },
                    { Name: "Gimbal", Type: "mck" },
                    { Name: "Slider", Type: "mck" },
                    { Name: "Jib", Type: "mck" },
                    { Name: "Drone", Type: "mck" },
                    { Name: "Dolly", Type: "mck" },
                    { Name: "Steadicam", Type: "mck" },
                    { Name: "Crane", Type: "mck" },
                    { Name: "Forward", Type: "direction" },
                    { Name: "Backward", Type: "direction" },
                    { Name: "Left", Type: "direction" },
                    { Name: "Right", Type: "direction" },
                    { Name: "Up", Type: "direction" },
                    { Name: "Down", Type: "direction" },
                    { Name: "Straight", Type: "track" },
                    { Name: "Circular", Type: "track" },



                ]
                $scope.groupEquipment = function (item) {
                    //alert(item);
                    return item.Type === 'mck' ? 'Mechanism' : (item.Type === 'direction' ? 'Direction' : (item.Type === 'track' ? 'Track' : ""));
                }

                $scope._MergeSceneId = { scene: null };
                $scope.MergeScene = () => {

                    $scope.MergesceneId.push($scope._MergeSceneId.scene.Id);

                    $http.post(root + 'api/ScenesandScript/MergeScene', $scope.MergesceneId).then(function success(response) {
                        if (response.status == 200) {
                            $rootScope.getscenes();
                            $scope.MergesceneId = [];
                            $scope._MergeSceneId = { scene: null };
                            $state.go($state.current, { sceneId: response.data })
                        }
                    }, function error() { });
                }
                $scope.getUserProfile();
                //$scope.SceneDetail();
            }
        }, 100);
    });
});

myApp.controller('CreateShotCtrl', function ($scope, $rootScope, $uibModal, $uibModalInstance, shot, $stateParams, $http, $timeout, toaster, title, projectItem, ProjectService, projectId, sceneId) {
    angular.element(document).ready(function () {
        $scope.files = [];
        $scope.sizelist = true;

        $scope.options = {
            inputClass: 'border-0',
            format: 'hexString',
            required: true,

        };

        $scope.title = title;
        $scope.SceneId = sceneId;
        $scope.ProjectId = projectId;
        $scope.cancel = function () {
            $uibModalInstance.dismiss();
        }
        $scope.checkempty = () => {
            if ($scope.Shot.Size.Name) {
                return true;
            } else if ($scope.Shot.Type.length > 0) {
                return true;
            }
            else if ($scope.Shot.Movement.Name) {
                return true;
            }
            else if ($scope.Shot.Equipment.length > 0) {
                return true;
            }
            else if ($scope.Shot.Vfx.length > 0) {
                return true;
            }
            else if ($scope.Shot.Camera.length > 0) {
                return true;
            }
            else if ($scope.Shot.Fps.Name) {
                return true;
            }
            else if ($scope.Shot.Lens.length > 0) {
                return true;
            }
            else if ($scope.Shot.SpecialEquipment.length > 0) {
                return true;
            } else if ($scope.Shot.Sound) {
                return true;
            } else if ($scope.Shot.Lighting) {
                return true;
            } else if ($scope.Shot.UnitId || $scope.Shot.UnitId > 0) {
                return true;
            } else { return false }
        }
        if (shot) {
            $scope.ViewImage = false;
            $scope.Shot = shot;
            if (shot.UnitId) {
                $http.get(root + 'api/ProjectUnits/GetProjectUnits?ProjectId=' + projectId).then(function success(response) {
                    $scope.projectUnits = response.data;
                    $scope.unitfiltered_pages = response.data;
                    angular.forEach($scope.projectUnits, function (item) {
                        if (item.Id == shot.UnitId) {
                            $scope.unit = item.Name;
                        }
                    });

                    console.log($scope.projectUnits);
                }, function error() { });
            }
        } else {
            $scope.Shot = {
                Color: '#FFFFFF',

                Type: [],

                Equipment: [],
                Vfx: [],
                Camera: [],
                Lens: [],
                isDeleted: false,
                Schedule_hh: 0,
                Schedule_mm: 0,
                SpecialEquipment: []
            }

            $scope.ViewImage = true;
        }
        $scope.size = [
            { Name: "Close-up", type: "closeup" },
            { Name: "Medium Close-up", type: "closeup" },
            { Name: "Extreme Close-up", type: "closeup" },
            { Name: "Wide Close-up", type: "closeup" },
            { Name: "Medium Shot", type: "medium" },
            { Name: "Close Shot", type: "medium" },
            { Name: "Medium Close Shot", type: "medium" },
            { Name: "Wide Shot", type: "long" },
            { Name: "Extreme Wide Shot", type: "long" },
            { Name: "Full Shot", type: "long" },
            { Name: "Medium Full Shot", type: "long" },
            { Name: "Long Shot", type: "long" },
            { Name: "Extreme Long Shot", type: "long" },
        ]
        $scope.groupSize = function (item) {
            //alert(item);
            return item.type === 'closeup' ? 'Close-ups' : (item.type === "medium" ? "Medium Shots" : (item.type === "long" ? "Long Shots" : ""));
        }

        $scope.type = [
            { Name: "Eye Level", type: "height" },
            { Name: "Low Angle", type: "height" },
            { Name: "High Angle", type: "height" },
            { Name: "Overhead", type: "height" },
            { Name: "Shoulder Level", type: "height" },
            { Name: "Hip Level", type: "height" },
            { Name: "Knee Level", type: "height" },
            { Name: "Ground Level", type: "height" },
            { Name: "Single", type: "framing" },
            { Name: "Two Shot", type: "framing" },
            { Name: "Three Shot", type: "framing" },
            { Name: "Over-The-Shoulder", type: "framing" },
            { Name: "Over-The-Hip", type: "framing" },
            { Name: "Point of View", type: "framing" },
            { Name: "Dutch(left)", type: "Dutch" },
            { Name: "Dutch(right)", type: "Dutch" },
            { Name: "Rack Focus", type: "focus" },
            { Name: "Shallow Focus", type: "focus" },
            { Name: "Deep Focus", type: "focus" },
            { Name: "Tilt-Shift", type: "focus" },
            { Name: "Zoom", type: "focus" },

        ]
        $scope.groupType = function (item) {
            //alert(item);
            return item.type === 'height' ? 'Camera Height' : (item.type === "framing" ? "Framing" : (item.type === "Dutch" ? "Dutch Angle" : (item.type === "focus" ? "Focus / DOF" : "")));
        }

        $scope.movement = [
            { Name: "Static", type: "movement" },
            { Name: "Pan", type: "movement" },
            { Name: "Tilt", type: "movement" },
            { Name: "Swish Pan", type: "movement" },
            { Name: "Swish Tilt", type: "movement" },
            { Name: "Tracking", type: "movement" },


        ]
        $scope.groupMovement = function (item) {
            //alert(item);
            return item.type === 'movement' ? 'Camera Movement' : "";
        }

        $scope.vfx = [
            { Name: "Motion Graphics", type: "vfx" },
            { Name: "Matte Painting", type: "vfx" },
            { Name: "Composite", type: "vfx" }


        ]
        $scope.groupVfx = function (item) {
            //alert(item);
            return item.type === 'vfx' ? 'VFX' : "";
        }
        $scope.camera = [
            { Name: "CAM A", type: "cam" },
            { Name: "CAM B", type: "cam" },
            { Name: "CAM C", type: "cam" },
            { Name: "CAM D", type: "cam" },
            { Name: "CAM E", type: "cam" },
            { Name: "CAM F", type: "cam" },
            { Name: "CAM G", type: "cam" },
            { Name: "CAM H", type: "cam" },
            { Name: "CAM I", type: "cam" }


        ]
        $scope.groupCamera = function (item) {
            //alert(item);
            return item.type === 'cam' ? 'Camera' : "";
        }
        $scope.lens = [
            { Name: "Normal", type: "view" },
            { Name: "Telephoto", type: "view" },
            { Name: "Wide Angle", type: "view" },
            { Name: "Fish-Eye", type: "view" },
            { Name: "Zoom", type: "view" },
            { Name: "10mm", type: "prime" },
            { Name: "12mm", type: "prime" },
            { Name: "14mm", type: "prime" },
            { Name: "16mm", type: "prime" },
            { Name: "18mm", type: "prime" },
            { Name: "20mm", type: "prime" },
            { Name: "24mm", type: "prime" },
            { Name: "25mm", type: "prime" },
            { Name: "28mm", type: "prime" },
            { Name: "30mm", type: "prime" },
            { Name: "32mm", type: "prime" },
            { Name: "35mm", type: "prime" },
            { Name: "40mm", type: "prime" },
            { Name: "50mm", type: "prime" },
            { Name: "65mm", type: "prime" },
            { Name: "75mm", type: "prime" },
            { Name: "85mm", type: "prime" },
            { Name: "100mm", type: "prime" },
            { Name: "135mm", type: "prime" },
            { Name: "150mm", type: "prime" },
            { Name: "180mm", type: "prime" },


        ]
        $scope.groupLens = function (item) {
            //alert(item);
            return item.type === 'view' ? 'Angle of View' : (item.type === 'prime' ? 'Primes' : "");
        }
        $scope.fps = [
            { Name: "24 fps", type: "fps" },
            { Name: "23.98 fps", type: "fps" },
            { Name: "25 fps", type: "fps" },
            { Name: "30 fps", type: "fps" },
            { Name: "29.97 fps", type: "fps" },
            { Name: "48 fps", type: "fps" },
            { Name: "47.95 fps", type: "fps" },
            { Name: "50 fps", type: "fps" },
            { Name: "59.94 fps", type: "fps" },
            { Name: "60 fps", type: "fps" },
            { Name: "75 fps", type: "fps" },
            { Name: "100 fps", type: "fps" },
            { Name: "120 fps", type: "fps" },
            { Name: "240 fps", type: "fps" },
            { Name: "300 fps", type: "fps" },

        ]
        $scope.groupFps = function (item) {
            //alert(item);
            return item.type === 'fps' ? 'Frame Rate' : "";
        }
        $scope.specialequipment = [
            { Name: "GoPro", type: "se" },
            { Name: "Drone", type: "se" },
            { Name: "High Speed", type: "se" },
            { Name: "360°VR", type: "se" },
            { Name: "Under-Water", type: "se" },
            { Name: "Hand Cracked", type: "se" },
            { Name: "3D", type: "se" },
            { Name: "IMAX", type: "se" },


        ]
        $scope.groupSpecialequipment = function (item) {
            //alert(item);
            return item.type === 'se' ? 'Special Equipment' : "";
        }
        $scope.equipment = [
            { Name: "Sticks", type: "mck" },
            { Name: "Hand Held", type: "mck" },
            { Name: "Gimbal", type: "mck" },
            { Name: "Slider", type: "mck" },
            { Name: "Jib", type: "mck" },
            { Name: "Drone", type: "mck" },
            { Name: "Dolly", type: "mck" },
            { Name: "Steadicam", type: "mck" },
            { Name: "Crane", type: "mck" },
            { Name: "Forward", type: "direction" },
            { Name: "Backward", type: "direction" },
            { Name: "Left", type: "direction" },
            { Name: "Right", type: "direction" },
            { Name: "Up", type: "direction" },
            { Name: "Down", type: "direction" },
            { Name: "Straight", type: "track" },
            { Name: "Circular", type: "track" },



        ]
        $scope.groupEquipment = function (item) {
            //alert(item);
            return item.type === 'mck' ? 'Mechanism' : (item.type === 'direction' ? 'Direction' : (item.type === 'track' ? 'Track' : ""));
        }

        //UNIT
        $scope.getProjectUnits = function () {
            $http.get(root + 'api/ProjectUnits/GetProjectUnits?ProjectId=' + projectId).then(function success(response) {
                $scope.projectUnits = response.data;


                console.log($scope.projectUnits);
            }, function error() { });
        }
        $scope.unitlist = true;
        $scope.unitcomplete = function (string) {
            //$scope.filtered_pages = null;
            if (string == null || string == "undefined") {
                $scope.pagelist = true;
            } else {
                $scope.unitlist = true;
                var output = [];
                angular.forEach($scope.projectUnits, function (item) {
                    if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                        output.push(item);
                    }
                });
                if (output.length > 0) {
                    $scope.unitlist = false;
                }
                else {
                    $scope.unitlist = true;
                }
                $scope.unitfiltered_pages = output;
            }
        }
        var unit_list_check = false;
        $scope.unit_view_list = () => {
            if (unit_list_check === true) {
                unit_list_check = false;
                $scope.unitfiltered_pages = null;
                $scope.unitfiltered_pages = $scope.projectUnits;
                $scope.unitlist = false;
            }
            else {
                unit_list_check = true;
                $scope.unitfiltered_pages = null;
                $scope.unitlist = true;
            }

        }
        var unitid = null;
        $scope.unitfillTextbox = function (string, id) {
            unitid = parseInt(id);
            $scope.unit = string;

            $scope.unitfiltered_pages = null;
            $scope.unitlist = true;
        }

        $scope.getProjectUnits();
        $scope.save = (type) => {

            $scope.Shot.UnitId = parseInt(unitid);
            $scope.Shot.SceneId = parseInt($scope.SceneId);
            $http.post(root + "api/ScenesandScript/CreateShot/", $scope.Shot).then(function (Resp) {
                if (Resp.status === 200) {
                    if (type == 0) {
                        toaster.pop({
                            type: 'success',
                            title: 'Success',
                            body: 'Shot created successfully!',
                        });
                        if ($scope.uploadedFiles) {
                            if ($scope.uploadedFiles.length > 0) {

                                console.log(Resp);

                                angular.forEach($scope.uploadedFiles, function (item) {

                                    item.ShotId = Resp.data.Id;
                                    item.SceneId = $scope.Shot.SceneId
                                });
                                $http.post(root + 'api/DocumentFiles/PostDocumentFiles', $scope.uploadedFiles).then(
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
                    } else {
                        toaster.pop({
                            type: 'success',
                            title: 'Success',
                            body: 'Shot updated successfully!',
                        });
                    }
                    $uibModalInstance.dismiss();
                    $rootScope._getscenes();
                }
            }, function error() { });
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
                    $scope.$apply(function () {
                        $scope.hasFilesinUppy = true;
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

        }

    });

});