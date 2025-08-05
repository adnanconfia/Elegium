myApp.controller('talent_details_ctrl', function ($scope, $rootScope, $filter, $http, $timeout, $uibModal, toaster, $ngConfirm, $stateParams, $state, orderByFilter) {
    $scope.eventsTab = [];
    $scope.events = [$scope.eventsTab];
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

            var charId = parseInt($stateParams.charId);
            $scope.charId = parseInt($stateParams.charId);
            var projectId = parseInt($stateParams.id);
            $scope.projectId = projectId;
            if (parseInt(charId) === 0) {
                window.location.href = root + "#/" + projectId + "/cast";
            }

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

            var docDefinition = {
            };
            var buildTableBody = () => {
                var body = [];

                var dataRow = [];
                dataRow.push({ text: 'General information', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                body.push(dataRow);

                dataRow = [];
                dataRow.push({ text: $scope.Actor.Name, margin: [5, 5, 5, 5], bold: true, style: 'small' })
                if ($scope.Actor.SecondStreet !== null || $scope.Actor.SecondCity !== null || $scope.Actor.SecondPostalCode !== null || $scope.Actor.SecondCountry.name !== null) {
                    dataRow.push({ text: 'Second Home', margin: [5, 5, 5, 5], bold: true, style: 'small' });
                } else {
                    dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                }

                if ($scope.Actor.ProdStreet != null || $scope.Actor.ProdCity != null || $scope.Actor.ProdPostalCode != null || $scope.Actor.ProdCountry.name != null) {
                    dataRow.push({ text: 'Home during production', margin: [5, 5, 5, 5], bold: true, style: 'small' });
                } else {
                    dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                }
                body.push(dataRow);
                dataRow = [];
                if ($scope.Actor.FirstStreet !== null || $scope.Actor.FirstCity !== null || $scope.Actor.FirstPostalCode !== null || $scope.Actor.FirstCountry.name !== null) {
                    var street = "";
                    var city = "";
                    var post = "";
                    var country = ""
                    if ($scope.Actor.FirstStreet !== null) {
                        street = $scope.Actor.FirstStreet;
                    }
                    if ($scope.Actor.FirstPostalCode !== null) {
                        post = $scope.Actor.FirstPostalCode + ",";
                    }
                    if ($scope.Actor.FirstCountry.name !== null) {
                        country = $scope.Actor.FirstCountry.name;
                    }
                    if ($scope.Actor.FirstCity !== null) {
                        city = $scope.Actor.FirstCity;
                    }
                    dataRow.push({ text: street + "\n" + post + city + "\n" + country, margin: [5, 5, 5, 5], style: 'small' });

                } else {
                    dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                }

                if ($scope.Actor.SecondStreet !== null || $scope.Actor.SecondCity !== null || $scope.Actor.SecondPostalCode !== null || $scope.Actor.SecondCountry.name !== null) {
                    street = "";
                    city = "";
                    post = "";
                    country = ""
                    if ($scope.Actor.SecondStreet !== null) {
                        street = $scope.Actor.SecondStreet;
                    }
                    if ($scope.Actor.SecondPostalCode !== null) {
                        post = $scope.Actor.SecondPostalCode + ",";
                    }
                    if ($scope.Actor.SecondCountry.name !== null) {
                        country = $scope.Actor.SecondCountry.name;
                    }
                    if ($scope.Actor.SecondCity !== null) {
                        city = $scope.Actor.SecondCity;
                    }
                    dataRow.push({ text: street + "\n" + post + city + "\n" + country, margin: [5, 5, 5, 5], style: 'small' });

                } else {
                    dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                }

                if ($scope.Actor.ProdStreet != null || $scope.Actor.ProdCity != null || $scope.Actor.ProdPostalCode != null || $scope.Actor.ProdCountry.name != null) {
                    street = "";
                    city = "";
                    post = "";
                    country = ""
                    if ($scope.Actor.ProdStreet !== null) {
                        street = $scope.Actor.ProdStreet;
                    }
                    if ($scope.Actor.ProdPostalCode !== null) {
                        post = $scope.Actor.ProdPostalCode + ",";
                    }
                    if ($scope.Actor.ProdCountry.name !== null) {
                        country = $scope.Actor.ProdCountry.name;
                    }
                    if ($scope.Actor.ProdCity !== null) {
                        city = $scope.Actor.ProdCity;
                    }
                    dataRow.push({ text: street + "\n" + post + city + "\n" + country, margin: [5, 5, 5, 5], style: 'small' });

                } else {
                    dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                }
                body.push(dataRow);
                dataRow = [];
                if ($scope.Actor.PhoneHome !== null) {
                    dataRow.push({ text: "Phone (home)", margin: [5, 5, 5, 5], bold: true, style: 'small' });
                } else {
                    dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                }
                if ($scope.Actor.PhoneMobile !== null) {
                    dataRow.push({ text: "Phone (mobile)", margin: [5, 5, 5, 5], bold: true, style: 'small' });
                } else {
                    dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                }
                if ($scope.Actor.Fax !== null) {
                    dataRow.push({ text: "Fax", margin: [5, 5, 5, 5], bold: true, style: 'small' });
                } else {
                    dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                }
                body.push(dataRow);
                dataRow = [];
                if ($scope.Actor.PhoneHome !== null) {
                    dataRow.push({ text: $scope.Actor.PhoneHome, margin: [5, 5, 5, 5], style: 'small' });
                } else {
                    dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                }
                if ($scope.Actor.PhoneMobile !== null) {
                    dataRow.push({ text: $scope.Actor.PhoneMobile, margin: [5, 5, 5, 5], style: 'small' });
                } else {
                    dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                }
                if ($scope.Actor.Fax !== null) {
                    dataRow.push({ text: $scope.Actor.Fax, margin: [5, 5, 5, 5], style: 'small' });
                } else {
                    dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                }
                body.push(dataRow);



                dataRow = [];
                if ($scope.Actor.Email !== null) {
                    dataRow.push({ text: "Email", margin: [5, 5, 5, 5], bold: true, style: 'small' });
                } else {
                    dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                }
                dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });

                body.push(dataRow);
                dataRow = [];
                if ($scope.Actor.PhoneHome !== null) {
                    dataRow.push({ text: $scope.Actor.Email, margin: [5, 5, 5, 5], style: 'small' });
                } else {
                    dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                }
                dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                body.push(dataRow);

                dataRow = [];
                if ($scope.Actor.AgencyId !== null) {
                    dataRow.push({ text: "Agency", margin: [5, 5, 5, 5], bold: true, style: 'small' });
                } else {
                    dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                }
                if ($scope.Actor.Union) {
                    dataRow.push({ text: "Union number (SAG, ACTRA)", margin: [5, 5, 5, 5], bold: true, style: 'small' });
                } else {
                    dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                }

                dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });

                body.push(dataRow);
                dataRow = [];
                if ($scope.Actor.AgencyId !== null) {
                    dataRow.push({ text: $scope.Actor.AgencyId.Name, margin: [5, 5, 5, 5], style: 'small' });
                } else {
                    dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                }
                if ($scope.Actor.Union) {
                    dataRow.push({ text: $scope.Actor.Union, margin: [5, 5, 5, 5], style: 'small' });
                } else {
                    dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                }

                dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });

                body.push(dataRow);

                dataRow = [];
                if ($scope.Actor.Note !== null) {
                    dataRow.push({ text: "Note", margin: [5, 5, 5, 5], bold: true, style: 'small' });
                    dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                    dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                    body.push(dataRow);
                }


                dataRow = [];
                if ($scope.Actor.Note !== null) {
                    dataRow.push({ text: $scope.Actor.Note, margin: [5, 5, 5, 5], style: 'small' });
                    dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                    dataRow.push({ text: '', margin: [5, 5, 5, 5], bold: true });
                    body.push(dataRow);
                }

                if ($scope.link.length > 0) {
                    dataRow = [];
                    dataRow.push({ text: 'Links', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                    body.push(dataRow);
                    dataRow = [];
                    dataRow.push({ text: 'Title', bold: true, style: "small", margin: [5, 2, 5, 2] }, { text: "Url", style: 'small', bold: true }, { text: "" });
                    body.push(dataRow);
                    angular.forEach($scope.link, function (item) {
                        dataRow = [];
                        dataRow.push({ text: item.Title, style: "small", margin: [5, 2, 5, 2] }, { text: item.Url, style: "small" }, { text: "" });
                        body.push(dataRow);
                    });
                }
                if ($scope.periods.length > 0) {
                    dataRow = [];
                    dataRow.push({ text: 'Off periods', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                    body.push(dataRow);
                    dataRow = [];
                    dataRow.push({ text: '', bold: true, style: "small", margin: [5, 2, 5, 2] }, { text: "", style: "small" }, { text: "Note" });
                    body.push(dataRow);
                    angular.forEach($scope.periods, function (item) {
                        dataRow = [];
                        var startDate = "";
                        var EndDate = "";
                        var Note = "";
                        if (item.StartDate) {
                            startDate = $filter('date')(item.StartDate, "dd/MM/yyyy");
                        }
                        if (item.EndDate) {
                            EndDate = " till " + $filter('date')(item.EndDate, "dd/MM/yyyy");
                        } if (item.StartDateTime) {
                            startDate = $filter('date')(item.StartDateTime, "dd/MM/yyyy, hh:mm a");
                        }
                        if (item.EndDateTime) {
                            EndDate = " till " + $filter('date')(item.EndDateTime, "dd/MM/yyyy, hh:mm a");
                        }
                        if (EndDate === "") {
                            EndData = " all day"
                        }
                        if (item.Note) {
                            Note = item.Note;
                        }
                        dataRow.push({ text: startDate + EndDate, style: "small", margin: [5, 2, 5, 2] }, { text: "", style: "small" }, { text: Note, style: "small" });
                        body.push(dataRow);
                    });
                }
                if ($scope.Actor.files.length > 0) {

                    dataRow = [];
                    dataRow.push({ text: 'Images', fillColor: '#E6E6E6', bold: true, fontSize: 12, margin: [5, 2, 5, 2] }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                    body.push(dataRow);
                }

                return body;
            }

            $scope.getUserProfile = function () {
                $http.get(root + 'api/UserProfiles/GetCurrentUserProfile').then(function success(response) {
                    console.log(response.data);
                    $scope.userProfile = response.data.UserProfile;


                    $scope.UserPhoto = "data:image/jpg;base64," + $scope.userProfile.Photo;

                    console.log($scope.userProfile);
                }, function error() { });
            }

            var getfiles = () => {

                var body = [];
                if ($scope.Actor.files.length > 0) {
                    //dataRow = [];
                    //dataRow.push({ text: ' Others', fillColor: '#E6E6E6', bold: true, fontSize: 12 }, { text: "", fillColor: '#E6E6E6' }, { text: "", fillColor: '#E6E6E6' });
                    //body.push(dataRow);
                    var dataRow = [];
                    extadd = 0;
                    var i = 0;
                    for (i = 0; i < $scope.Actor.files.length; i++) {
                        var img = window.location.protocol + "//" + window.location.host + '/api/UserProfiles/GetFileThumbnail/' + $scope.Actor.files[i].FileId + '/200/200';

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
            $scope.downloadPdf = () => {
                pdfMake.createPdf(docDefinition).download($scope.project.Name + "-" + $scope.Actor.Name + ".pdf");
                //pdfMake.createPdf(docDefinition).open();
            }
            $scope.getProject = () => {

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

                                        text: [$scope.project.Name + "\n", { text: $scope.Actor.Name + "\n", bold: true }, { text: today, style: 'small', bold: true }
                                        ]
                                    },
                                    {
                                        style: 'small',
                                        width: '*',
                                        alignment: 'right',
                                        text: [

                                            { text: $scope.userProfile.CompanyName, bold: false },

                                            { text: " ", bold: false },

                                            { text: " ", bold: false },
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
            $scope.getactor = () => {
                $scope.Link = {
                    "ActorId": $scope.charId,
                    "Is_Deleted": false
                }
                $scope.Period = {
                    "ActorId": $scope.charId,
                    "Is_Deleted": false
                }
                $http.get(root + 'api/ScenesandScript/GetTalent?projectid=' + projectId).then(function (response) {
                    if (response.status === 200) {


                        response.data.forEach(function (item) {
                            if (item.Id === $scope.charId) {
                                item.files = [];
                                $http.get(root + 'api/ScenesandScript/GetTalentFiles/' + parseInt(item.Id) + '/' + $scope.filesPaging.page + '/' + $scope.filesPaging.size).then(resp => {
                                    $scope.defaultImage = resp.data.FileId;
                                    $scope.files1 = resp.data.list;
                                    item.FileCount = $scope.files1.length;
                                    //angular.forEach($scope.files1, function (item1) {
                                    //    if (item1.Default === true) {
                                    //        item.Default = item1;

                                    //    }
                                    //});
                                    if (item.FileCount > 0) {

                                        item.HasFile = true;
                                        item.files = $scope.files1;
                                        //item.FileId = $scope.Files[0].FileId;
                                    } else {
                                        item.HasFile = false;
                                    }

                                }, err => {
                                });
                                $scope.Actor = item;
                                $scope.Actor.Is_talent = true;

                                $scope.Actor.FirstCountry = { "name": item.FirstCountry };
                                $scope.Actor.SecondCountry = {
                                    "name": item.SecondCountry
                                };
                                $scope.Actor.ProdCountry = {
                                    "name": item.ProdCountry
                                };
                                $scope.getAgency();
                                $scope.getProjects();
                                $scope.getComments()
                                initializeTaskObj();
                                $scope.getLink();
                                $scope.getPeriod();
                                $scope.getProject();
                                $scope.getUserProfile();

                            }
                        });
                    }
                }, function (error) { });
            }

            $scope.showDel = (id, type) => {

                $scope.charIndex = id + type;
            }
            $scope.Editinfo = false;
            $scope.show_Edit_info = () => {
                $scope.Editinfo = true;
            }
            $scope.getAgency = () => {
                $http.get(root + 'api/ScenesandScript/GetAgency?projectid=' + projectId).then(function (response) {
                    if (response.status === 200) {
                        $scope.agency = [];
                        angular.forEach(response.data, function (item) {
                            $scope.agency.push(item.Agency);
                            if (item.Agency.Id === $scope.Actor.AgencyId) {
                                $scope.Actor.AgencyId = item.Agency;
                            }
                        });
                        $scope.filtered_agency = $scope.agency;
                    }
                }, function (error) { });
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
                $scope.Actor.AgencyId = agency;
                $scope._agency = agency.Name;
                $scope.agencyBtn = false;
            }

            $scope.agencyBtn = false;
            $scope.CreatAgencyBtn = true;
            $scope.showAgency = () => {
                $scope.agencyBtn = true;
            }
            $scope.hideAgency = () => {
                $scope.agencyBtn = false;
            }
            $scope.getProjects = function () {
                $http.get(root + 'api/Projects/GetProjects').then(function success(response) {
                    angular.forEach(response.data, function (item) {
                        if (item.Project.Id === projectId) {
                            $scope.Project = item.Project;
                        }
                    });
                }, function error() { });
            }

            $scope.showDel = (id, type) => {

                $scope.charIndex = id + type;
            }
            $scope.Editinfo = false;
            $scope.show_Edit_info = () => {
                $scope.Editinfo = true;
            }
            $scope.UpdateActor = (actor) => {
                if (actor.Is_talent == true) {
                    if (actor.AgencyId) {
                        actor.AgencyId = actor.AgencyId.Id;
                    }
                    if (actor.FirstCountry) {
                        var Country = actor.FirstCountry.name
                        actor.FirstCountry = Country;

                    } if (actor.SecondCountry) {
                        var sCountry = actor.SecondCountry.name
                        actor.SecondCountry = sCountry;

                    } if (actor.ProdCountry) {
                        var pCountry = actor.ProdCountry.name
                        actor.ProdCountry = pCountry;

                    }
                    actor.ProjectId = projectId;
                    delete actor.Is_talent;

                    $http.post(root + 'api/scenesandscript/CreateTalent/', actor).then(function (resp) {
                        $rootScope.getactor();
                        $scope.getactor();
                    }, error => { });
                    actor.Is_talent = true;
                } else {
                    $http.get(root + 'api/ScenesandScript/TalentToActor/' + parseInt($scope.charId)).then(
                        function success(resp) {
                            $scope.Editinfo = false;
                            $rootScope.GetOnBoarding();
                            $rootScope.getTalent();
                            $rootScope.getactor();
                            $state.go('cast.actor_details', { charId: resp.data })
                        }, function error() { });
                }
                $scope.Editinfo = false;
                $scope.EditSkills = false;
                $scope.EditSize = false;
                $scope.EditNote = false;
            }
            $scope.EditNote = false;
            $scope.show_note = () => {
                $scope.EditNote = true;
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

                                item.TalentId = parseInt($scope.charId);

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

                                        $rootScope.getTalent();
                                        $scope.getactor();
                                    }
                                }

                                , function error() { });
                        }
                    }
                });
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

                $scope.link = [];
                $scope.periods = [];
                $scope.openUrl = (url) => {
                    var win = window.open(url, '_blank');
                    win.focus();
                }
                $scope.deleteLink = (id) => {
                    $scope.Link.Is_Deleted = true;
                    $scope.Link.Id = id;
                    $http.post(root + 'api/scenesandscript/CreateLink', $scope.Link).then(
                        function success(resp) {
                            $scope.getLink();
                            $scope.getactor();
                        }, function error() { });
                }
                $scope.deletePeriod = (id) => {
                    $scope.Period.Is_Deleted = true;
                    $scope.Period.Id = id;
                    $http.post(root + 'api/scenesandscript/CreatePeriod', $scope.Period).then(
                        function success(resp) {
                            $scope.getPeriod();
                            $scope.getactor();
                        }, function error() { });
                }

                $scope.hideSplit = () => {
                    $scope.charIndex = -1;
                }
                $scope.EditLink = false;
                $scope.EditPeriod = false;
                $scope.Link = {
                    "TalentId": $scope.charId,
                    "Is_Deleted": false
                }
                $scope.Period = {
                    "TalentId": $scope.charId,
                    "Is_Deleted": false
                }
                $scope.getLink = () => {

                    $http.get(root + 'api/scenesandscript/GetLinksByTalentId/' + $scope.charId).then(
                        function success(resp) {
                            $scope.link = resp.data;
                        }, function error() { });
                }
                $scope.getPeriod = () => {
                    $http.get(root + 'api/scenesandscript/GetPeriodsByTalentId/' + $scope.charId).then(
                        function success(resp) {
                            $scope.periods = resp.data;
                        }, function error() { });
                }
                $scope.create_link = () => {
                    $http.post(root + 'api/scenesandscript/CreateLink', $scope.Link).then(
                        function success(resp) {
                            $scope.link.push(resp.data);
                            $scope.getactor();
                            $scope.EditLink = false;
                        }, function error() { });
                }
                $scope.create_Period = () => {
                    $http.post(root + 'api/scenesandscript/CreatePeriod', $scope.Period).then(
                        function success(resp) {
                            $scope.periods.push(resp.data);
                            $scope.getactor();
                            $scope.EditPeriod = false;
                        }, function error() { });
                }
                $scope.showDel = (id, type) => {

                    $scope.charIndex = id + type;
                }
                $scope.hideDel = () => {
                    $scope.charIndex = -1;
                }
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
                    });
                }
                $scope.DeleteActor = (Actor) => {
                    if (Actor.AgencyId) {
                        Actor.AgencyId = Actor.AgencyId.Id;
                    }
                    if (Actor.FirstCountry) {
                        var Country = Actor.FirstCountry.name
                        Actor.FirstCountry = Country;

                    } if (Actor.SecondCountry) {
                        var sCountry = Actor.SecondCountry.name
                        Actor.SecondCountry = sCountry;

                    } if (Actor.ProdCountry) {
                        var pCountry = Actor.ProdCountry.name
                        Actor.ProdCountry = pCountry;

                    }
                    Actor.Is_deleted = true;
                    delete Actor.Default;
                    delete Actor.files;
                    $http.post(root + 'api/scenesandscript/CreateTalent/', Actor).then(function (resp) {
                        if (resp.status === 200) {
                            $rootScope.getactor();
                            window.location.href = root + "#/" + projectId + "/cast";
                        }
                    }, function error() { });
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
                                return $scope.Actor.Default;
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
                $scope.show_link = () => {
                    $scope.EditLink = true;
                }
                $scope.show_period = () => {
                    $scope.EditPeriod = true;
                }
                $scope.cancel_period = () => {
                    $scope.EditPeriod = false;
                }
                $scope.cancel_link = () => {
                    $scope.EditLink = false;
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
                    $scope.Actor.files.splice(index, 1);

                $http.post(root + 'api/DocumentFiles/DeleteDocumentFiles/' + file.Id).then(resp => {
                    //$scope.getLinks();
                    $scope.tabContentLength.filesCount = $scope.tabContentLength.filesCount - 1;
                    if ($scope.tabContentLength.filesCount < 0) {
                        $scope.tabContentLength.filesCount = 0;
                    }

                    $rootScope.getactor();
                    $scope.getactor();
                }, err => {
                });

            }
            $scope.EditSkills = false;
            $scope.show_Skills_info = () => {
                $scope.EditSkills = true;
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

                    TalentId: parseInt($scope.charId)
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


                        if (item.TalentId === parseInt($scope.charId)) {
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
                TalentId: parseInt($scope.charId)
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
                            TalentId: parseInt($scope.charId)
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
                $http.get(root + 'api/ScenesandScript/GetTalentComments/' + parseInt($scope.charId)).then(resp => {
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
            $scope.EditSize = false;
            $scope.show_sizes_info = () => {
                $scope.EditSize = true;
            }
            $scope.checkFields = () => {
                if (!Array.isArray($scope.Actor)) {
                    var arr = Object.keys($scope.Actor)
                    for (var i = 41; i <= 77; i++) {
                        var s = arr[i];
                        if ($scope.Actor[arr[i]]) {
                            //$scope.EditSize = true;
                            return true;
                        }
                    }

                    return false;
                }
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

            $scope.getactor();

        }, 100);
    });
});
