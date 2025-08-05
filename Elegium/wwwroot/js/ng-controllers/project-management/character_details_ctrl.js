

myApp.controller('character_details_ctrl', function ($scope, $rootScope, $filter, $http, $timeout, $uibModal, toaster, $ngConfirm, $stateParams, $state, orderByFilter, uiCalendarConfig) {

    $scope.eventsTab = [];
    $scope.events = [$scope.eventsTab];
    //Configure Calendar -mubi calendar  
    $scope.uiConfig = {
        calendar: {
            height: 'auto',
            editable: true,
            timeFormat: 'H:mm',
            forceEventDuration: true,
            displayEventTime: true,
            header: {
                left: 'prev',
                center: 'title',
                right: 'next'
            },
            selectable: true,
            //select:  function (date, allDay) {
            //    $(".bg-gray").removeClass("bg-gray");
            //   $(this).addClass("bg-gray");
            //    //alert(jsEvent.target.cellIndex);
            //    //if ($(jsEvent.target).hasClass("fc-widget-content")) {
            //    //    $(jsEvent.target).addClass("bg-gray");
            //    //} else {
            //    //    if ($(jsEvent.target.parentNode).hasClass("fc-widget-content") || $(jsEvent.target.parentNode).hasClass("fc-day-top")) { $(jsEvent.target.parentNode).addClass("bg-gray"); $(jsEvent.target).addClass("bg-gray"); }
            //    //}
            //}
        }

    };
 
    angular.element(document).ready(function () {
        $timeout(function () {
        
            //console.log($stateParams);
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

            var charId = $stateParams.charId;
            $scope.charId = $stateParams.charId;
            var projectId = $stateParams.id;
            $scope.projectId = projectId;
            if (parseInt(charId) === 0) {
                window.location.href = root + "#/" + projectId + "/cast";
            }
            $scope.showDel = (id, type) => {

                $scope.charIndex = id + type;
            }
            $scope.hideDel = () => {
                $scope.charIndex = -1;
            }
            var docDefinition = {
            };

            var buildTableBody = () => {
                var body = [];

                var dataRow = [];
                dataRow.push({ text: 'General information', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                body.push(dataRow);

                dataRow = [];
                dataRow.push({ text: $scope.char.Name, margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] }, { text: '', margin: [5, 5, 5, 5] });
                body.push(dataRow);
                if ($scope.char.files.length > 0) {

                    dataRow = [];
                    dataRow.push({ text: 'Moods and Images', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                    body.push(dataRow);
                }

                return body;
            }

            $scope.marked = function (_mark) {
                $scope.char.marked = _mark;
                $scope.UpdateChar($scope.char);
            }
            $scope.images;
            $scope.Character = { char: null };
            $scope.Merge = () => {
                $http.get(root + 'api/ScenesandScript/GetCharacters?projectid=' + projectId).then(function success(response) {

                    $scope.filtered_char = response.data.filter(function (item) {
                        if (item.Id != $scope.char.Id) {
                            return item;
                        }
                    });

                }, function error() { });
           
                $('#myModal').modal('show');
            }
            $scope.MergeChar = (chid2, chid) => {
          
                $http.get(root + 'api/ScenesandScript/MergeCharacter/' + chid + '/' + chid2).then(function success(response) {

                    $http.delete(root + 'api/ScenesandScript/DeleteChar/' + chid).then(function success(response) {
                        if (response.status === 200) {
                          
                            $rootScope.GetOnBoarding();
                            $rootScope.get_char();
                            $scope.Character = { char: null };
                            $state.go($state.current, { charId: chid2 });
                        }

                    }, function error() { });
                
                }, function error() { });

            }
            $scope.DelAll = false;
            $scope.charCheck = false;
            $scope.toggleMulti = () => {
                if ($scope.charCheck) {
                    $scope.charCheck = false;
                } else {
                    $scope.charCheck = true;
                }
                $scope.checkSelected();
                $scope.deselectAll();
            }
            $scope.checkSelected = () => {
                var chk = false;
                angular.forEach($scope.files, function (item) {
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
                angular.forEach($scope.files, function (item) {
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
            $scope.toggleCharCheck = (char) => {
                if (char.check) {
                    char.check = false;
                } else {
                    char.check = true;
                }
                $scope.checkSelected();

            }
            $scope.deleteFileSelected = function () {
                $scope.files.forEach(function (file) {
                    var index = $scope.files.indexOf(file);
                    if (index > -1 && file.check == true) {
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
                });
            }
              $scope.selectAll = () => {
        angular.forEach($scope.files, function (item) {
            item.check = true;
        })
        $scope.checkSelected();
            }
            $scope.dowloadZipSelected = async () => {
                var zip = new JSZip();

                var img = zip.folder($scope.char.Name + "(Character)");

                if ($scope.files.length > 0) {
                    for (var i = 0; i < $scope.files.length; i++) {
                        if ($scope.files[i].check) {
                            const imageBlob = await fetch("/api/UserProfiles/GetFileThumbnail/" + $scope.files[i].FileId).then(response => response.blob());
                            const imgData = new File([imageBlob], $scope.char.files[i].Name);
                            img.file($scope.files[i].FileId + "-" + $scope.char.files[i].Name, imgData, { base64: true });
                        }
                    }
                }
                var today = new Date();
                today = $filter('date')(today, "yyyy-MMMM-dd-hh:mm a");
                zip.generateAsync({ type: "blob" })
                    .then(function (content) {
                        // see FileSaver.js
                        saveAs(content, $scope.char.Name + "-" + today);
                    });
            }
            $scope.dowloadZip = async  () => {
                var zip = new JSZip();
          
                var img = zip.folder($scope.char.Name + "(Character)");
            
                if ($scope.files.length > 0) {
                    for (var i = 0; i < $scope.files.length; i++) {
             
                        const imageBlob = await fetch("/api/UserProfiles/GetFileThumbnail/" + $scope.files[i].FileId).then(response => response.blob());
                        const imgData = new File([imageBlob], $scope.char.files[i].Name );
                        img.file($scope.files[i].FileId + "-" +$scope.char.files[i].Name, imgData, {base64:true});
                    }
                }
                var today = new Date();
                today = $filter('date')(today, "yyyy-MMMM-dd-hh:mm a");
                zip.generateAsync({ type: "blob" })
                    .then(function (content) {
                        // see FileSaver.js
                        saveAs(content, $scope.char.Name +"-" +today + ".zip");
                    });
            }
            $scope.downloadPdf = () => {
                pdfMake.createPdf(docDefinition).download($scope.project.Name + "-" + $scope.char.Name + ".pdf");
                //pdfMake.createPdf(docDefinition).open();
            }
       
            //FIlespdf
            var getfiles =  () => {

                var body = [];
                if ($scope.char.files.length > 0) {
                    //dataRow = [];
                    //dataRow.push({ text: ' Others', fillColor: '#E6E6E6', bold: true, fontSize: 12 }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                    //body.push(dataRow);
                    var dataRow = [];
                    extadd = 0;
                    var i = 0;
                    for (i = 0; i < $scope.char.files.length; i++) {
                        var img = window.location.protocol + "//" + window.location.host + '/api/UserProfiles/GetFileThumbnail/' + $scope.char.files[i].FileId + '/200/200';

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
            var buildSuggestion = () => {
                var i = 1;
                var body = [];
                var dataRow = [];
               
                angular.forEach($scope.newCharacterTalent1, function (item) {
                    dataRow = [];
                    dataRow.push({ text: "Suggested " + i + " : " + item.Name, fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] })
                    body.push(dataRow)
                    dataRow = [];
                    dataRow.push({ text: 'Agency \n' + item.Agency }, { text: '' }, { text: '' })
                    body.push(dataRow)
                    i++;
                });
                return body;
            }

            $scope.viewActor = (actor) => {
                if (actor.ActorId) {
                    $state.go('cast.actor_details', { charId: actor.ActorId})
                } else {
                    $state.go('cast.talent_details', { charId: actor.TalentId })
                }
            }
            $scope.getProject = () => {
                var Agency = "-";
                if ($scope.Is_Cast.length > 0) {

                    if ($scope.Is_Cast[0].TalentId) {
                        $http.get(root + 'api/ScenesandScript/GetAgencyNameByTalentId?id=' + $scope.Is_Cast[0].TalentId).then(function success(response) { Agency = response.data }, error = () => { });
                    } else {
                        $http.get(root + 'api/ScenesandScript/GetAgencyNameByActorId?id=' + $scope.Is_Cast[0].ActorId).then(function success(response) { Agency = response.data }, error = () => { });

                    }
                }
                else if ($scope.newCharacterTalent1.length > 0 && $scope.Is_Cast.length <= 0) {
                    var i = 1;
                    angular.forEach($scope.newCharacterTalent1, function (item) {
                        item.Agency = "-";
                        if (item.TalentId) {
                            $http.get(root + 'api/ScenesandScript/GetAgencyNameByTalentId?id=' + item.TalentId).then(function success(response) {
                                item.Agency = response.data

                            }, error = () => { });
                        } else {
                            $http.get(root + 'api/ScenesandScript/GetAgencyNameByActorId?id=' + item.ActorId).then(function success(response) { item.Agency = response.data }, error = () => { });

                        }
                    });
                }
                $http.get(root + 'api/Projects/GetProject/' + projectId).then(function success(response) {
                    $scope.project = response.data.Project;
                    var today = new Date();
                    today = $filter('date')(today, "EEEE , MMMM dd,yyyy hh:mm a");
                     var files = getfiles();
                    $scope.images = files;
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

                    var actors = null;
                    if (!$scope.char.Sugggestion) {
                        if ($scope.Is_Cast.length > 0) {

                        actors = {

                            alignment: 'justify',
                            margin: [0, 0, 0, 10],

                            style: 'table',
                            table: {
                                margin: [0, 0, 0, 10],
                                widths: ["33%", "33%", "33%"],
                                body:
                                    [

                                        [{ text: $scope.Is_Cast[0].Name, fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }],
                                        [{ text: 'Agency \n' + Agency }, { text: '' }, { text: '' }],

                                    ]


                            }, layout: 'noBorders'
                        }
                    } else if ($scope.newCharacterTalent1.length > 0 && $scope.Is_Cast.length <= 0) {

                        actors = {

                            alignment: 'justify',
                            margin: [0, 0, 0, 10],

                            style: 'table',
                            table: {
                                margin: [0, 0, 0, 10],
                                widths: ["33%", "33%", "33%"],
                                body: buildSuggestion()



                            }, layout: 'noBorders'

                        }
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

                                        text: [$scope.project.Name + "\n", { text: $scope.char.Name + "\n", bold: true }, { text: today, style: 'small', bold: true }
                                        ]
                                    },
                                    {
                                        style: 'small',
                                        width: '*',
                                        alignment: 'right',
                                        text: [

                                            { text: $scope.userProfile.CompanyName , bold: false },
                                          
                                            { text:" ", bold: false },
                                          
                                            { text: " ", bold: false },
                                            ]
                                    },
                                    {
                                        style: 'small',
                                        width: '*',
                                        text: [

                                            { text:  $scope.findSymbolForClass('.fa-user'), style: 'symbol', bold: false },
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
                            actors
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
            $rootScope.char = () => {
                $scope.getChar();
                $scope.getCharFiles();
                initializeTaskObj();
            }
            $scope.getChar = async function () {
               
                $scope.Suggested = [];
                $scope.suggestbtn = true;
                $scope.Is_Cast = [];
                $scope.Is_Rejected = [];
                $scope.Rating = [];
                $scope.newCharacterTalent = [];
                $scope.newCharacterTalent1 = [];
                await $http.get(root + 'api/ScenesandScript/GetCharacters?projectid=' + projectId).then(function success(response) {

                    console.log(response);
                    $scope.chars = response.data;
                    if (response.data.length <= 0) {
                        window.location.href = root + "#/" + projectId + "/cast";
                    }
                    $scope.filtered_chars = $scope.chars.filter(function (item) {

                        if (item.Id == charId) {
                            //console.log(exist, item.Id);
                            item._ext = false;
                            return item;

                        }
                    });
                    $scope.char = $scope.filtered_chars[0];
                    $scope.char.files = [];
                    $scope.char._ext = false;


                }, function error() { });

                await $http.get(root + 'api/ScenesandScript/GetTalent?projectid=' + projectId).then(function (response) {
                    if (response.status === 200) {
                        $scope.Talent = response.data;

                        $scope.Talent.forEach(function (item) {
                            item.TalentId = item.Id;
                            console.log(item);
                            $http.get(root + 'api/ScenesandScript/GetTalentFiles/' + parseInt(item.Id) + '/' + $scope.filesPaging.page + '/' + $scope.filesPaging.size).then(resp => {
                                $scope.defaultImage = resp.data.FileId;
                                $scope.files1 = resp.data.list;
                                item.FileCount = $scope.files1.length;
                                angular.forEach($scope.files1, function (item1) {
                                    if (item1.Default === true) {
                                        item.Default = item1;

                                    }
                                });
                                if (item.FileCount > 0) {
                                    console.log($scope.files1);
                                    item.HasFile = true;
                                    item.file = $scope.files1[0];
                                    //item.FileId = $scope.Files[0].FileId;
                                } else {
                                    item.HasFile = false;
                                }
                            }, err => {
                            });
                            $scope.Suggested.push(item);
                        });
                    }
                }, function (error) { });
                await $http.get(root + 'api/ScenesandScript/GetActor?projectid=' + projectId).then(function (response) {
                    if (response.status === 200) {
                        $scope.Actor = response.data;

                        $scope.Actor.forEach(function (item) {
                            item.ActorId = item.Id;
                            $http.get(root + 'api/ScenesandScript/GetActorFiles/' + parseInt(item.Id) + '/' + $scope.filesPaging.page + '/' + $scope.filesPaging.size).then(resp => {
                                $scope.defaultImage = resp.data.FileId;
                                $scope.files1 = resp.data.list;
                                item.FileCount = $scope.files1.length;
                                angular.forEach($scope.files1, function (item1) {
                                    if (item1.Default === true) {
                                       item.Default = item1;

                                    }
                                });
                                if (item.FileCount > 0) {
                                    console.log($scope.files1);
                                    item.HasFile = true;
                                    item.file = $scope.files1[0];
                                    //item.FileId = $scope.Files[0].FileId;
                                } else {
                                    item.HasFile = false;
                                }
                            }, err => {
                            });
                            $scope.Suggested.push(item);
                        });
                    }
                }, function (error) { });

                await $http.get(root + 'api/ScenesandScript/GetCharacterTalentByChId?charId=' + charId).then(function (response) {
                    if (response.status === 200) {
                        console.log(response.data);
                        $scope.CharacterTalent = response.data;
                        $scope.newCharacterTalent = [];
                        if ($scope.CharacterTalent.length > 0) {
                            var arrayWithIds = $scope.CharacterTalent.map(function (x) {
                                return x.ActorId;
                            });
                            $scope.Suggested = $scope.Suggested.filter(function (item) {
                                var present = arrayWithIds.indexOf(item.ActorId) != -1;
                                if (!present) {
                                    return item;
                                } else {
                                    item1 = item;
                                    angular.forEach($scope.CharacterTalent, function (item2) {
                                        if (item.ActorId === item2.ActorId) {
                                            item1.Is_CastFixed = item2.Is_CastFixed;
                                            item1.Rating = item2.Rating;
                                            item1.CharacterTalentId = item2.Id
                                            item1.Is_Rejected = item2.Is_Rejected
                                        }
                                    });
                                    $scope.newCharacterTalent.push(item);
                                }

                            });
                            arrayWithIds = $scope.CharacterTalent.map(function (x) {
                                return x.TalentId;
                            });
                            $scope.Suggested = $scope.Suggested.filter(function (item) {
                                var present = arrayWithIds.indexOf(item.TalentId) != -1;
                                if (!present) {
                                    return item;
                                } else {
                                    item1 = item;
                                    item1 = item;
                                    angular.forEach($scope.CharacterTalent, function (item2) {
                                        if (item.TalentId === item2.TalentId) {
                                            item1.Is_CastFixed = item2.Is_CastFixed;
                                            item1.Rating = item2.Rating;
                                            item1.CharacterTalentId = item2.Id
                                            item1.Is_Rejected = item2.Is_Rejected
                                        }
                                    });

                                    $scope.newCharacterTalent.push(item);
                                }

                            });
                            angular.forEach($scope.newCharacterTalent, function (item) {
                                if (item.Is_CastFixed) {
                                    $scope.Is_Cast.push(item);
                                }
                                else if (item.Is_Rejected) {
                                    $scope.Is_Rejected.push(item);
                                }
                                else {
                                    $scope.newCharacterTalent1.push(item);
                                }
                                if (item.Rating) {
                                    $scope.Rating.push(item);
                                }
                            });
                            if ($scope.Is_Cast.length > 1) {
                                $scope.char.GroupOfCharacters = true;
                            }


                        }
                        $scope.Suggested1 = $scope.Suggested;
                        $scope.getComments();
                    
                    }
                }, function (error) { });
                $scope.getCharFiles();
                $scope.getProject();
                $rootScope.get_char();
            }

            $scope.getUserProfile = function () {
                $http.get(root + 'api/UserProfiles/GetCurrentUserProfile').then(function success(response) {
                    console.log(response.data);
                    $scope.userProfile = response.data.UserProfile;


                    $scope.UserPhoto = "data:image/jpg;base64," + $scope.userProfile.Photo;

                    console.log($scope.userProfile);
                }, function error() { });
            }
            $scope.getUserProfile();
            $scope.getChar();
            $scope.Editinfo = false;
            $scope.show_Edit_info = () => {
                $scope.Editinfo = true;
            }
            $scope._ext = false;
            $scope.UpdateChar = (newch) => {
                if (newch._ext === false) {
                    delete newch._ext;
                    $http.post(root + 'api/ScenesandScript/CreateChar', newch).then(
                        function success(resp) {
                            $scope.Editinfo = false;
                            $rootScope.GetOnBoarding();
                            $scope.getChar();
                        }, function error() { });
                    $scope.Editinfo = false;
                    newch._ext = false;
                } else {
                    $http.get(root + 'api/ScenesandScript/CharToExtra/'+ parseInt($scope.charId) ).then(
                        function success(resp) {
                            $scope.Editinfo = false;
                            $rootScope.GetOnBoarding();
                            $rootScope.get_char();
                            $state.go('cast.extra_details', { charId: resp.data.Id })
                        }, function error() { });
                    $scope.Editinfo = false;
                    newch._ext = false;

                }
            }
            $scope.suggestbtn = true;
            $scope.dropdown = -1;
            $scope.ShowDropDown = (id, type) => {
                if ($scope.dropdown === -1 || id + type != $scope.dropdown) {
                    $scope.dropdown = id + type;
                } else {
                    $scope.dropdown = -1;
                }
            }

            $scope.add_suggested = (c) => {
                var data = {
                    "Rating": 0,
                    "ActorId": null,
                    "CharId": parseInt(charId),
                    "TalentId": null
                }
                if (c.ActorId) {
                    data.ActorId = c.ActorId;
                } else if (c.TalentId) {
                    data.TalentId = c.TalentId;
                }
                $http.post(root + 'api/ScenesandScript/PostCharacterTalent', data).then(
                    function success(resp) {
                        $scope.suggestbtn = true;
                        $rootScope.GetOnBoarding();
                        $scope.getChar();
                    }, function error() { });
            }

            $scope.complete_Actor = function (e, string) {
                if (string == null || string == "undefined") {

                    $scope.suggestbtn = true;
                    $scope.Suggested = $scope.Suggested1;
                } else if (e.keyCode === 13) {
                    $scope.create_Talent(string);
                } else {

                    var output = [];
                    angular.forEach($scope.Suggested, function (item) {

                        if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                            output.push(item);
                            //var exist = $scope.EditScene.CharacterId.includes(item.Id.toString());

                            //if (exist) {
                            //    chk = true;

                            //}
                        }
                    });
                    if (output.length > 0) {
                        $scope.suggestbtn = true;
                        $scope.Suggested = output;
                    }
                    if (output.length == 0) {
                        $scope.Suggested = output;
                        $scope.suggestbtn = false;
                    }


                }

            }
    
            $scope.DeleteChTalent = (ch) => {
                var data = {
                    "charactersTalentId": ch.CharacterTalentId
                }
                $http.delete(root + 'api/ScenesandScript/DeleteCharacterTalent/' + ch.CharacterTalentId, data).then(
                    function success(resp) {
                        $scope.charIndex = -1;
                        $rootScope.GetOnBoarding();
                        $scope.getChar();
                    }, function error() { });
            }
            $scope.UpdateChTalent = (ch) => {
                var data = {
                    "Id": ch.CharacterTalentId,
                    "Rating": ch.Rating,
                    "ActorId": null,
                    "CharId": parseInt($scope.charId),
                    "TalentId": null,
                    "Is_CastFixed": ch.Is_CastFixed
                }
                if (ch.ActorId) {
                    data.ActorId = ch.ActorId;
                } else if (ch.TalentId) {
                    data.TalentId = ch.TalentId;
                }
                $http.post(root + 'api/ScenesandScript/PostCharacterTalent', data).then(
                    function success(resp) {

                        $rootScope.GetOnBoarding();
                        $scope.getChar();
                    }, function error() { });
            }
            $scope.UpdateChTalentRating = (ch) => {
                var data = {
                    "Id": ch.CharacterTalentId,
                    "Rating": ch.Rating,
                    "ActorId": null,
                    "CharId": parseInt($scope.charId),
                    "TalentId": null,
                    "Is_CastFixed": ch.Is_CastFixed
                }
                if (ch.ActorId) {
                    data.ActorId = ch.ActorId;
                } else if (ch.TalentId) {
                    data.TalentId = ch.TalentId;
                }
                $http.post(root + 'api/ScenesandScript/PostCharacterTalent', data).then(
                    function success(resp) {

                      
                    }, function error() { });
            }
            $scope.UpdateChTalentCast = (ch) => {
                var data = {
                    "Id": ch.CharacterTalentId,
                    "Rating": ch.Rating,
                    "ActorId": null,
                    "CharId": parseInt($scope.charId),
                    "TalentId": null,
                    "Is_CastFixed": true
                }
                if (ch.ActorId) {
                    data.ActorId = ch.ActorId;
                } else if (ch.TalentId) {
                    data.TalentId = ch.TalentId;
                }
                $http.post(root + 'api/ScenesandScript/PostCharacterTalent', data).then(
                    function success(resp) {

                        $rootScope.GetOnBoarding();
                        $scope.getChar();
                    }, function error() { });
            }


            $scope.Unfix = (ch) => {
                var data = {
                    "Id": ch.CharacterTalentId,
                    "Rating": ch.Rating,
                    "ActorId": null,
                    "CharId": parseInt($scope.charId),
                    "TalentId": null,
                    "Is_CastFixed": false
                }
                if (ch.ActorId) {
                    data.ActorId = ch.ActorId;
                } else if (ch.TalentId) {
                    data.TalentId = ch.TalentId;
                }
                $http.post(root + 'api/ScenesandScript/PostCharacterTalent', data).then(
                    function success(resp) {
                        $scope.charIndex = -1;
                        $rootScope.GetOnBoarding();
                        $scope.getChar();
                    }, function error() { });
            }
            $scope.RejectSuggestion = (ch) => {
                var data = {
                    "Id": ch.CharacterTalentId,
                    "Rating": ch.Rating,
                    "ActorId": null,
                    "CharId": parseInt($scope.charId),
                    "TalentId": null,
                    "Is_CastFixed": ch.Is_CastFixed,
                    "Is_Rejected": true
                }
                if (ch.ActorId) {
                    data.ActorId = ch.ActorId;
                } else if (ch.TalentId) {
                    data.TalentId = ch.TalentId;
                }
                $http.post(root + 'api/ScenesandScript/PostCharacterTalent', data).then(
                    function success(resp) {
                        $scope.charIndex = -1;
                        $rootScope.GetOnBoarding();
                        $scope.getChar();
                    }, function error() { });
            }
            $scope.AgainSuggestion = (ch) => {
                var data = {
                    "Id": ch.CharacterTalentId,
                    "Rating": ch.Rating,
                    "ActorId": null,
                    "CharId": parseInt($scope.charId),
                    "TalentId": null,
                    "Is_CastFixed": ch.Is_CastFixed,
                    "Is_Rejected": false
                }
                if (ch.ActorId) {
                    data.ActorId = ch.ActorId;
                } else if (ch.TalentId) {
                    data.TalentId = ch.TalentId;
                }
                $http.post(root + 'api/ScenesandScript/PostCharacterTalent', data).then(
                    function success(resp) {
                        $scope.charIndex = -1;
                        $rootScope.GetOnBoarding();
                        $scope.getChar();
                    }, function error() { });
            }

            //Create-Agency
            var modalInstance = null;
            $scope.create_Talent = function (ch) {
                modalInstance = $uibModal.open({
                    animation: false,
                    templateUrl: root + 'js/ng-templates/Cast/create-CommonTalent.html',
                    controller: 'CreateCommontTalentCtrl',
                    size: 'lg',
                    backdrop: 'static',
                    resolve: {
                        title: function () {
                            return 'Create actor or talent ';
                        },
                        projectItem: function () {
                            return null;
                        },
                        projectId: function () {
                            return $scope.projectId;
                        },
                        Name: function () {
                            return ch;
                        },
                         Agency: function () {
                            return "";
                        }

                    }
                });
                modalInstance.result.then(function () {

                }, function (data) {
                        $scope.suggestbtn = false;
                        $scope.getChar();
                });
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
            $scope.crop = (docCat) => {
                var modalInstance = $uibModal.open({
                    animation: $scope.animationsEnabled,
                    templateUrl: root + 'js/ng-templates/croppieWithUrl.html',
                    controller: 'croppieWithUrlCtrl',
                    size: 'lg',
                    resolve: {
                        title: function () {
                            return "Adjust photo view";
                        },
                        width: function () { return 200 },
                        height: function () { return 200 },
                        imgUrl: function () {
                            return '/api/UserProfiles/DownloadFile/' + docCat.FileId;
                        }
                    }
                });
                modalInstance.result.then(function () {
                    //on ok button press 
                }, function (data) {
                    document.getElementById('img' + docCat.Id).style.backgroundImage = 'url(' + data + ')';
                    $http.post(root + 'api/DocumentCategories/PostFileThumbnail', {
                        FileId: docCat.FileId,
                        FileArray: data
                    }).then(
                        resp => {
                        },
                        err => {
                        });
                });}

            $scope.openProjectFileDialog = function () {
                $scope.isItDocDacThumbnail = false;
                $('#projectFile').click();
            }
            $scope.openProjectFileDialog1 = function () {
                //$scope.isItDocDacThumbnail = true;
                //$('#projectFile').click();
                //document.getElementById('idDocCatThumbnailFile').click();
                var modalInstance = $uibModal.open({
                    animation: false,
                    templateUrl: root + 'js/ng-templates/croppie-playground.html',
                    controller: 'CroppiePlayCtrlCustom',
                    size: 'lg',
                    resolve: {
                        title: function () {
                            return "Select Thumbnail";
                        },
                        image: function () {
                            return $scope.char.Default;
                        },
                        width: function () { return 750 },
                        height: function () { return 300 }
                    }
                });
                modalInstance.result.then(function () {
                    //on ok button press 
                }, function (data) {
                        let promiseSource = fetch(data.source);
                        let promiseImage = fetch(data.image);
                        Promise.all([promiseSource, promiseImage]).then((results) => {
                            let sourceBlobPromise = results[0].blob();
                            let imageBlobPromise = results[1].blob();
                            Promise.all([sourceBlobPromise, imageBlobPromise]).then((blobs) => {
                                const file1 = new File([blobs[0]], "source.png", { type: "image/png" });
                                const file2 = new File([blobs[1]], "thumbnail.png", { type: "image/png" });
                                addFilesToUppy([file1, file2]);
                            });
                        });
                });
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
                        fileObj.Default = file.name == 'thumbnail.png' ? true : false;
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

                                item.CharId = parseInt($scope.charId);

                            });
                            $http.post(root + 'api/DocumentFiles/PostCastDocumentFiles', $scope.uploadedFiles).then(
                                function success(resp) {
                                    console.log(resp.data);
                                    if (resp.data.length > 0) {

                                        $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount + $scope.uploadedFiles.length;
                                        resp.data.forEach((file) => {
                                            $scope.files.push(file);

                                            if (file.Default)
                                                $scope.defaultImage = file.FileId;
                                        });
                                        $scope.uppy.reset();
                                        $scope.files = [];
                                        $scope.getCharFiles(); $rootScope.get_char();
                                    }
                                }
                              
                                , function error() { });
                        }
                    }
                });
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
            $scope.getCharFiles = function () {
                $scope.files = [];
                $http.get(root + 'api/ScenesandScript/getCharFiles/' + parseInt($scope.charId) + '/' + $scope.filesPaging.page + '/' + $scope.filesPaging.size).then(resp => {
                    $scope.defaultImage = resp.data.FileId;
                    if (resp.data.list.length > 0) {
                        $scope.files = resp.data.list;
                        $scope.char.files = $scope.files;
                        angular.forEach($scope.files, function (item) {
                            if (item.Default === true) {
                                $scope.char.Default = item;
                               
                            }
                        });
                    } else {
                        $scope.files = [];
                    }
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
         
            $scope.DeleteChar = (id) => {
                $http.delete(root + 'api/ScenesandScript/DeleteChar/' + id).then(function success(response) {
                    if (response.status === 200) {
                        $scope.chars = $scope.chars.filter(function (item) {

                            if (item.Id != id) {
                                return item;
                            }
                        });
                        $rootScope.GetOnBoarding();
                        $rootScope.get_char();
                        window.location.href = root + "#/" + projectId + "/cast";
                    }

                }, function error() { });
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
             
                    CharId: parseInt($scope.charId)
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


                        if (item.CharId == parseInt($scope.charId)) {
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
            initializeTaskObj();

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
            $scope.commentObj = {
                Text: "",
                newComment: true,
                AnnouncementId: null,
                MentionUsers: null,
                CharId: parseInt($scope.charId)
            };
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
                            CharId: parseInt($scope.charId)
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
            $scope.comments = [];
     

            $scope.getComments = function () {
                //GetComments
                $http.get(root + 'api/ScenesandScript/GetCharComments/' + parseInt($scope.charId)).then(resp => {
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
        }, 200);
    });

});
myApp.controller('CreateCommontTalentCtrl', function ($scope, $rootScope, $filter, $http, $uibModal, $uibModalInstance, $timeout, toaster, $ngConfirm, $stateParams, $state, title, projectId, Name, Agency) {
    angular.element(document).ready(function () {
        $timeout(function () {
        $scope.title = title;
        console.log(Agency);

        $scope.cancel = () => {
            $uibModalInstance.dismiss();
        }

        $scope.files = [];
        $scope.actor = { FirstCountry: null, SecondCountry: null, ProdCountry: null, AgencyId: null, AgeMin: 0, AgeMax: 110 };
        if (Agency) {
            $scope.actor.AgencyId = Agency;
        }
        $scope.actor.Name = Name;
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
        $scope.getAgency();
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
        $scope.Is_talent = false;
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
            $scope.actor.ProjectId = parseInt(projectId);
            if ($scope.Is_talent === false) {


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

                    }
                }, function error() { });
            } else {
                delete $scope.actor.Reel;
                $http.post(root + 'api/scenesandscript/CreateTalent/', $scope.actor).then(function (resp) {
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

                    }
                }, function error() { });
            }
            $scope.cancel();
            if (Agency) {
                $rootScope.getagency();
            } else {
                $rootscope.char();
            }
       
        }
    
        }, 100)
    });
});

myApp.directive('starRating', function () {
    return {
       
        template: '<ul class="rating">' +
            '<li ng-repeat="star in stars" ng-class="star" ng-click="toggle($index)">' +
            '\u2605' +
            '</li>' +
            '</ul>',
        scope: {
            ratingValue: '=',
            max: '=',
            onRatingSelected: '&',
            _enabled:'='
        },
        link: function (scope, elem, attrs) {
            console.log(attrs)
            var updateStars = function () {
                scope.stars = [];
                for (var i = 0; i < scope.max; i++) {
                    scope.stars.push({
                        filled: i < scope.ratingValue
                    });
                }
            };

            scope.toggle = function (index) {
                if (attrs.enabled==="1") {
                    scope.ratingValue = index + 1;
                    scope.onRatingSelected({
                        rating: index + 1
                    });
                }
            };

            scope.$watch('ratingValue', function (oldVal, newVal) {
                //if (newVal) {
                updateStars(); 
                //}
            });
            scope.$watch('_enabled', function (oldVal, newVal) {
                //if (newVal) {
                //scope._enabled = newVal;
                //}
            });
        }
    }

});