//#region Confiatech Scenes Controller
myApp.controller('ScenesController', function ($scope, $rootScope, $filter,$timeout, $http, $uibModal, toaster, $ngConfirm, $stateParams, $state) {

    console.log('$stateParams', $stateParams);
    //$scope.unitId = $stateParams.id;
    $scope.projectId = $stateParams.id;
    var projectId = parseInt($stateParams.id);
    //$scope.users = [];
    //$scope.allUsers = [];
    //$scope.isEditModeUser = false;
    var modalInstance = null;
    $scope.createscene = function () {
        modalInstance = $uibModal.open({
            animation: false,
            templateUrl: root + 'js/ng-templates/scenesandscript/create-scene.html',
            controller: 'CreateSceneCtrl',
            size: 'lg',
            backdrop: 'static',
            resolve: {
                title: function () {
                    return 'Create Scene ';
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
    $scope.sceneIndex = -1;
    $scope.showSpilt = (id,type) => {
        $scope.sceneIndex = id+type;
    }
    $scope.hideSplit = () =>{
        $scope.sceneIndex = -1;
}
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

    $scope.gotoSceneProfile = function (sceneId) {
        $state.go('scenesandscripts.scene_details', { sceneId: sceneId });
    }

    $scope.findSymbolForClass = (selector)=> {
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
     var findCSSRuleContent=function(mySheet, selector) {
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
    var stripQuotes=(string)=>{
        var len = string.length;
        return string.slice(1, len - 1);
    }

    var docDefinition = {
    };



    $scope.openPdf = function () {
        pdfMake.createPdf(docDefinition).open();
    };

    $rootScope.downloadPdf = function () {
        pdfMake.createPdf(docDefinition).download($scope.project.Name+".pdf");
    };
    $rootScope.className = (id) => {
        if (id == 1) {
            return 'bg-white';
        } else if (id == 2) {
            return 'bg-Dayext';
        }else if (id == 3) {
            return 'bg-lightyellow';
        }else if (id == 4) {
            return 'bg-Nightext';
        }
    }
    var character = [];
    var display_char = (scenes) => {
        var string = "";
        angular.forEach(scenes, function (item) {
            angular.forEach(item.character, function (item1) {
                var index = character.indexOf(item1.Id) !=-1;
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
     var buildTableBody=()=> {
         var body = [];


         $scope.scenes.forEach(function (row) {
             var dataRow =[];
             var bg_color="#fff";
             var color= "#000";
             if (row.scene.environment_id == 2) {
                 bg_color = '#61ad81';
                 color = "#FFF";

             } else if (row.scene.environment_id == 3) {
                 bg_color = '#f4e485';
                 color = "#000";

             }else if (row.scene.environment_id == 4) {
                 bg_color = '#7675bc';
                 color = "#fff";

             }
             dataRow.push({ text: row.scene.Index.toString(), fillColor: bg_color, color: color});
             dataRow.push({ text: row.Env_name.toString() + " " + row.Set_name, fillColor: bg_color, color: color });
             var string = "";
             if (row.CharacterId.length > 0) {
                string+= "Cast: ";
             }
                angular.forEach(row.CharacterId, function (item1) {
                  
                  
                        string += item1+",";
                        
                    
                });
             if (row.CharacterId.length > 0) {
                 string += "\n";
             }
             if (row.ExtraId.length > 0) {
                string+= "Extras: ";
             }
             angular.forEach(row.ExtraId, function (item1) {
                  
                  
                        string += item1+",";
                        
                    
             });
             if (row.ExtraId.length > 0) {
                 string += "\n";
             }
             string += "Scheduled duration: " + row.scene.scheduled_hh + ":" + row.scene.scheduled_mm + "\n";
             if (row.scene.unit) {
                 string += "Unit: ";
             }
            
             
             angular.forEach($scope.units, function (item) {
                 if (item.Id == row.scene.unit) {
                     string += item.Name;
                 }
             });
          
             if (row.scene.unit) {
                 string += "\n";
             }
             dataRow.push({ text: string, fillColor: bg_color, color: color });
            body.push(dataRow);
        });

        return body;
    }
    $scope.getProject = () => {
        $http.get(root + 'api/Projects/GetProject/' + projectId).then(function success(response) {

            console.log(response,"projects");
            $scope.project = response.data.Project;
            var today = new Date();
            today = $filter('date')(today, "EEEE , MMMM dd,yyyy hh:mm a");
            docDefinition = {
                content: [

                    {
                        alignment: 'justify',
                        margin: [0, 0, 0, 100],
                        columns: [
                            {
                                width: 90,
                                alignment: 'left',

                                text: ""

                            },
                            {
                                width: '*',
                                alignment: 'left',

                                text: [$scope.project.Name + "\n", { text: "Scenes\n", bold: true }, { text: today, style: 'small', bold: true }
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
                        columns: [
                            {
                                width: 90,
                                alignment: 'left',

                                text: ""

                            },
                            {
                                width: '*',
                                alignment: 'left',

                                text: [{ text: "Cast:\n", bold: true },{ text: display_char($scope.scenes),bold:true ,style:"small"}]
                            },

                            {
                                style: 'small',
                                width: '*',
                                text: [""]
                            },
                        ]
                    },
                    {
                        pageBreak: 'before',
                        alignment: 'justify',
                        margin: [0, 0, 0, 10],
                        	
                        style: 'table',
                        table: {
                            widths: ["33%", "33%", "33%"],
                            body: buildTableBody()
                                
                            
                        }
                   
                    },

                ],
                styles: {
                    header: {
                        bold: true,
                        color: '#000',
                        fontSize: 11
                    },
                    small: {
                        font:'Roboto',
                        fontSize: 9
                    },
                    symbol: {
                        font: 'FontAwesome'
                    },
                    txt_left:{
                        alignment: 'left',
                    },	table: {
                        margin: [0, 5, 0, 15],
                        fontSize: 9

                    }
                }
            }
        }, function error() { });
    }
    $rootScope.getscenes = function () {
        $http.get(root + 'api/ScenesandScript/GetScenes?projectId='+ projectId).then(function success(response) {

            console.log(response);
            $scope.scenes = response.data;
            $scope.scenes1 = response.data;
            $scope.getProject();
            $scope.getunits();
           
            
        
        }, function error() { });
    }
    $rootScope.getscenes();
    $scope.srchbtn = false;
    $scope.checksearch = () => {
        if ($scope.srchbtn) {
            return 'fa fa-times';
        } else {

            return 'fa fa-search';
        }
    }

    $scope.Split = (id) => {
        $http.get(root + 'api/ScenesandScript/SplitScene?id=' + id).then(function success(response) {
            $rootScope.getscenes();
            $scope.$broadcast('allscenes');
            if (response.status === 200) {
                $scope.sceneIndex = -1;
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: 'Success!',
                });
            }
            $scope.sceneIndex = -1;
            return true;
        }, function error() { });
    }
   

    $scope.Merge = (id,index) => {
        $scope.MergesceneId = [];
        $scope.MergesceneId.push(id);
        $scope.MergeSceneIndex = null;
        $scope.MergeSceneIndex = index;

        $scope.filtered_scenes = $scope.scenes.filter(function(item) {
            if (item.Id != id) {
                return item;
            }
        });
        $('#myModal').modal('show');
    }
    $scope._MergeSceneId = { scene :null };
    $scope.MergeScene = () => {

        $scope.MergesceneId.push($scope._MergeSceneId.scene.Id);

        $http.post(root + 'api/ScenesandScript/MergeScene', $scope.MergesceneId ).then(function success(response) {
            $rootScope.getscenes();
            $scope.MergesceneId = [];
            $scope._MergeSceneId = { scene: null };
        }, function error() { });
    }
    $scope.duplicate = (id) => {
        $http.get(root + 'api/ScenesandScript/DuplicateScene?id=' + id).then(function success(response) {
            $rootScope.getscenes();
            $scope.$broadcast('allscenes');
            if (response.status === 200) {
                toaster.pop({
                    type: 'success',
                    title: 'Success',
                    body: 'Duplicate Scene created successfully!',
                });
                $scope.sceneIndex = -1;
            }
            return true;
        }, function error() { });
    }
    $scope.show_search = () => {
        if ($scope.srchbtn) {
            $scope.srchbtn = false;
        } else {
            $scope.srchbtn = true;
        }
    }
    $rootScope.deleteScene = (id) => {
        $http.delete(root + 'api/ScenesandScript/DeleteScene/' + id).then(function success(response) {
            $scope.sceneIndex = -1;
            $rootScope.getscenes();
        }, function error() { });
    }

    $scope.search=(string)=>{
        if (string == null || string == "undefined") {
            $scope.scenes = $scope.scenes1;
        } else {
            output = []
            angular.forEach($scope.scenes1, function (item) {
                if (!item.scene.Description) {
                    item.scene.Description = "";
                }
                if (!item.Env_name) {
                    item.scene.Description = "";
                }
                if (item.Env_name.toLowerCase().indexOf(string.toLowerCase()) >= 0 || item.scene.Description.toLowerCase().indexOf(string.toLowerCase()) >= 0 || item.scene.Index.toLowerCase().indexOf(string.toLowerCase()) >= 0 || item.Set_name.toLowerCase().indexOf(string.toLowerCase()) >= 0 ) {
                    output.push(item);
                }
            });

            $scope.scenes = output;
        }
    }
    $scope.EsId = 0;
    $scope.sIndex = -1;
    $scope.show_Es_dropdown = (scene,type,index) => {
        if ($scope.EsId ===0) {
            
            $scope.EsId = scene.Id + type;
            $scope.sIndex = index;
            if (type == "ch") {
                $scope.getcharacters();
            }
            if (type == "ex") {
                $scope.getextra();
            } if (type == "env") {
                $scope.getenv();
            }if (type == "set") {
                $scope.getset();
            }
        } else {
            
            $scope.EsId = 0;
            $scope.sIndex = -1;
            $scope.createScene(scene);
        }
    }
   
    $scope.createScene = (scene) => {
        if (scene.ScriptDay) {
            scene.ScriptDay = parseInt(scene.ScriptDay);
        }
        scene.environment_id = parseInt(scene.environment_id);
        var allscenes = {

            "Id": scene.Id,
            "scene": scene


        }

        $http.post(root + 'api/ScenesandScript/CreateScene/', (allscenes)
        ).then(response => {
            if (response.status == 200) {
                $rootScope.getscenes();
            }
        }, function error() { });
    }

    $scope.pagelist = true;
    $scope.unitlist = true;
    var list_check = true;

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
    $scope.fillTextbox = function (string,scene) {
        $scope.scenes[$scope.sIndex].scene.ScriptPages = string;
        $scope.filtered_pages = null;
        $scope.pagelist = true;
        $scope.EsId = 0;
        $scope.sIndex = -1;
        $scope.createScene(scene);
    }

    
    //Characters
  
    $scope.charbtn = true;
    $scope.getcharacters = function () {
        $http.get(root + 'api/ScenesandScript/GetCharacters?projectid=' + projectId).then(function success(response) {

            console.log(response);
            $scope.chars = response.data;

            $scope.filtered_chars = $scope.chars.filter(function (item) {
                var exist = $scope.scenes[$scope.sIndex].CharacterId.includes(item.Id.toString());

                if (!exist) {
                    //console.log(exist, item.Id);
                    return item;
                }
            });
            console.log($scope.chars);

           
        }, function error() { });
    }
    $scope.removeCh = function (id,sceneId) {
        var data = {
            "CharacterId": id,
            "SceneId": parseInt(sceneId)

        }
        $http.post(root + 'api/ScenesandScript/DeleteChar', data).then(function success(response) {
            console.log(response);
            $rootScope.getscenes();
            $timeout(function () { $scope.getcharacters(); }, 100);
          
           

        }, function error() { });
    }
    $scope.createchar = (ch,scene) => {
        
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
                    var arrayWithIds = $scope.scenes[$scope.sIndex].character.map(function (x) {
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
                   
                    var exist = $scope.scenes[$scope.sIndex].character.includes(item.Id.toString());

                    if (exist) {
                        chk = true;

                    } else {
                        output.push(item);

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
    $scope.add_char = (id,sceneid) => {
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
            "SceneId": parseInt(sceneid),
            "CharacterId": id
        }
        $http.post(root + 'api/ScenesandScript/AddCharacter/', (scenecharacter)
        ).then(response => {
            if (response.status == 200) {
                $rootScope.getscenes();
            }
        }, err => {

        });

    }
    $scope.removeExt = function (id,sceneId) {
        var data = {
            "ExtraId": id,
            "SceneId": parseInt(sceneId)

        }
        $http.post(root + 'api/ScenesandScript/DeleteExtra', data).then(function success(response) {
            //console.log(response);
            $rootScope.getscenes();
            $timeout(function () { $scope.getextra(); }, 100);
            

        }, function error() { });
    }
    $scope.extbtn = true;
    $scope.getextra = function () {
        $http.get(root + 'api/ScenesandScript/GetExtras?projectid=' + projectId).then(function success(response) {

            console.log(response);
            $scope.ext = response.data;
           
           

            $scope.filtered_ext = $scope.ext.filter(function (item) {
                var exist = $scope.scenes[$scope.sIndex].ExtraId.includes(item.Id.toString());

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

                    var arrayWithIds = $scope.scenes[$scope.sIndex].extra.map(function (x) {
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
    $scope.complete_ext = function (e, string,sceneId) {
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
                   
                    var exist = $scope.scenes[$scope.sIndex].ExtraId.includes(item.Id.toString());

                    if (exist) {
                        chk = true;

                    } else {
                        output.push(item);
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
    $scope.add_ext = (id,sceneId) => {
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
            "SceneId": parseInt(sceneId),
            "ExtraId": id
        }
        $http.post(root + 'api/ScenesandScript/AddExtra/', (sceneExtra)
        ).then(response => {
            if (response.status == 200) {
                $rootScope.getscenes();
            }
        }, err => {

        });

    }

    $scope.getenv = function () {
        $http.get(root + 'api/ScenesandScript/GetEnvironment?projectid=' + projectId).then(function success(response) {


            $scope.env = response.data;
        }, function error() { });
    }
    $scope.getset = function () {
        $http.get(root + 'api/ScenesandScript/GetSets?projectid=' + projectId).then(function success(response) {


            $scope.set = response.data;
            $scope.filtered_set = response.data;
        }, function error() { });
    }
    $scope.show_set_check = true;
    $scope.toggle_set = () => {
        if ($scope.show_set_check) {
            $scope.show_set_check = false;
        } else {
            $scope.show_set_check = true;
        }
    }
   
    $scope.setbtn = true;
    $scope.set_set = (Id, Set_name,scene) => {
        $scope.show_set_check = true;
        scene.setId = parseInt(Id);
        $scope.SetName = Set_name;
        $scope.createScene(scene);
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
});


//#region end Confiatech Scenes Controller
//#region Confiatech CreateScene Controller
myApp.controller('CreateSceneCtrl', function ($scope, $rootScope, $uibModal, $uibModalInstance, $stateParams, $http, $timeout, toaster, title, projectItem, ProjectService, projectId) {
        angular.element(document).ready(function () {
            $timeout(function () {
                console.log("create", $scope);
                $scope.title = title;



                $scope.getcharacters = function () {
                    $http.get(root + 'api/ScenesandScript/GetCharacters?projectid=' + projectId).then(function success(response) {

                        console.log(response);
                        $scope.chars = response.data;
                        $scope.filtered_chars = response.data;
                        for (var i = 0; i < $scope.filtered_chars.length; i++) {
                            $scope.filtered_chars[i].index = i + 1;
                        }
                    }, function error() { });
                }
                $scope.getcharacters();
                $scope.GetExtras = function () {
                    $http.get(root + 'api/ScenesandScript/GetExtras?projectid=' + projectId).then(function success(response) {

                        console.log(response);
                        $scope.ext = response.data;
                        $scope.filtered_ext = response.data;

                        $scope.filtered_ext = response.data;
                        for (var i = 0; i < $scope.filtered_ext.length; i++) {
                            $scope.filtered_ext[i].index = i + 1;
                        }
                    }, function error() { });
                }
                $scope.GetExtras();


                $scope.getenv = function () {
                    $http.get(root + 'api/ScenesandScript/GetEnvironment?projectid=' + projectId).then(function success(response) {


                        $scope.env = response.data;
                    }, function error() { });
                }
                $scope.getenv();
                $scope.getset = function () {
                    $http.get(root + 'api/ScenesandScript/GetSets?projectid=' + projectId).then(function success(response) {


                        $scope.set = response.data;
                        $scope.filtered_set = response.data;
                    }, function error() { });
                }
                $scope.getset();
                $scope.addedchar = []
                $scope.add_char_check = true;
                $scope.char_to_add = true;


                $scope.show_add_char = () => {
                    $scope.char_to_add = true;
                }
                $scope.show_char = () => {
                    $scope.char_to_add = false;
                }
                $scope.add_char = (id,index) => {
                    $scope.filtered_chars.forEach(function (item) {
                        //console.log(item)
                        if (item.Id == id) {
                            
                            $scope.addedchar.push(item);
                        }
                    });
                    $scope.filtered_chars = $scope.filtered_chars.filter(function (item) {
                        if (item.Id != id) {
                            return item;
                        }
                    });

                    //console.log($scope.addedchar);
                    if ($scope.addedchar.length > 0) {
                        $scope.add_char_check = false;
                    } else {
                        $scope.add_char_check = true;;
                    }
                    if ($scope.filtered_chars.length > 0) {
                        $scope.char_to_add = false;
                    } else {
                        $scope.char_to_add = true;
                    }
                }

                $scope.remove_char = (id) => {
                    $scope.addedchar.forEach(function (item) {
                        //console.log(item)
                        if (item.Id == id) {

                            $scope.filtered_chars.push(item);
                        }
                    });
                    $scope.addedchar = $scope.addedchar.filter(function (item) {
                        if (item.Id != id) {
                            return item;
                        }
                    });

                    //console.log($scope.addedchar);
                    if ($scope.addedchar.length > 0) {
                        $scope.add_char_check = false;
                    } else {
                        $scope.add_char_check = true;
                    }
                    if ($scope.filtered_chars.length > 0) {
                        $scope.char_to_add = false;
                    } else {
                        $scope.char_to_add = true;
                    }
                }
                $scope.charbtn = true;
                $scope.extbtn = true;
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
                                var arrayWithIds = $scope.addedchar.map(function (x) {
                                    return x.Id;
                                });

                                $scope.filtered_chars = $scope.filtered_chars.filter(function (item) {
                                    var present = arrayWithIds.indexOf(item.Id) != -1;
                                    if (!present) {
                                        return item;
                                    }

                                });
                                $scope.charbtn = true;
                            }
                        }, function error() { });

                }
                $scope.complete_char = function (e,string) {
                    //$scope.filtered_pages = null;
                    if (!string) {
                        $scope.char_to_add = false;
                        $scope.charbtn = true;
                        //$scope.filtered_chars = $scope.chars;
                    } else if (e.keyCode === 13) {
                        $scope.createchar(string);
                    }else {

                        var output = [];
                        angular.forEach($scope.filtered_chars, function (item) {

                            if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                output.push(item);
                            }
                        });
                        if (output.length > 0) {
                            $scope.charbtn = true;
                        }
                        else {
                            $scope.charbtn = false;
                        }
                        $scope.filtered_chars = output;
                    }
                }
              
                //Extras 
                $scope.addedext = []
                $scope.add_ext_check = true;
                $scope.ext_to_add = true;
                //projectId

                $scope.show_add_ext = () => {
                    $scope.ext_to_add = true;
                }
                $scope.show_ext = () => {
                    $scope.ext_to_add = false;
                }
                $scope.add_ext = (id) => {
                    $scope.filtered_ext.forEach(function (item) {
                        //console.log(item)
                        if (item.Id == id) {

                            $scope.addedext.push(item);
                        }
                    });
                    $scope.filtered_ext = $scope.filtered_ext.filter(function (item) {
                        if (item.Id != id) {
                            return item;
                        }
                    });

                    //console.log($scope.addedchar);
                    if ($scope.addedext.length > 0) {
                        $scope.add_ext_check = false;
                    } else {
                        $scope.add_ext_check = true;;
                    }
                    if ($scope.filtered_ext.length > 0) {
                        $scope.ext_to_add = false;
                    } else {
                        $scope.ext_to_add = true;
                    }
                }

                $scope.remove_ext = (id) => {
                    $scope.addedext.forEach(function (item) {
                        //console.log(item)
                        if (item.Id == id) {

                            $scope.filtered_ext.push(item);
                        }
                    });
                    $scope.addedext = $scope.addedext.filter(function (item) {
                        if (item.Id != id) {
                            return item;
                        }
                    });

                    //console.log($scope.addedchar);
                    if ($scope.addedext.length > 0) {
                        $scope.add_ext_check = false;
                    } else {
                        $scope.add_ext_check = true;
                    }
                    if ($scope.filtered_ext.length > 0) {
                        $scope.ext_to_add = false;
                    } else {
                        $scope.ext_to_add = true;
                    }
                }
                $scope.createext = (ext) => {
                    var newext = {
                        "Name": ext,
                        "Project_Id": projectId
                    }
                    $scope.ext_srch = "";
                    $http.post(root + 'api/ScenesandScript/Createext', newext).then(
                        function success(resp) {
                            if (resp.status === 200) {
                                var item = resp.data;
                                $scope.ext.push(item);
                                item.index = $scope.ext.length;
                                $scope.filtered_ext = $scope.ext;
                             
                                var arrayWithIds = $scope.addedext.map(function (x) {
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
                $scope.complete_ext = function (e,string) {
                    //$scope.filtered_pages = null;
                    if (!string) {
                        $scope.ext_to_add = false;
                        $scope.extbtn = true;
                        //$scope.filtered_ext = $scope.ext;
                    } else if (e.keyCode === 13) {
                        $scope.createext(string);
                    }
                    else {

                        var output = [];
                        angular.forEach($scope.filtered_ext, function (item) {
                            if (item.Name.toLowerCase().indexOf(string.toLowerCase()) >= 0) {
                                output.push(item);
                            }
                        });
                        if (output.length > 0) {
                            $scope.extbtn = true;
                        }
                        else {
                            $scope.extbtn = false;
                        }
                        $scope.filtered_ext = output;
                    }
                }
       
                //Extra
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
                    $scope.Scene.ScriptPages = string;
                    $scope.filtered_pages = null;
                    $scope.pagelist = true;
                }

                $scope.getProjectUnits = function () {
                    $http.get(root + 'api/ProjectUnits/GetProjectUnits?ProjectId=' + projectId).then(function success(response) {
                        $scope.projectUnits = response.data;
                        $scope.unitfiltered_pages = response.data;
                        console.log($scope.projectUnits);
                    }, function error() { });
                }
                $scope.getProjectUnits();
                $scope.createUnit = (string) => {
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
                            $scope.Scene.unit = "";
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
                $scope.unitcomplete = function (e,string) {
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
                    unitid = parseInt(id);
                    $scope.Scene.unit = string;

                    $scope.unitfiltered_pages = null;
                    $scope.unitlist = true;
                }
                $scope.cancel = function () {
                    $uibModalInstance.dismiss();
                }
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

                $scope.Scene = {
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
                    isDeleted: false

                    //,

                    //Index: Index,
                    //environment_id: environment_id,
                    //point_in_time: point_in_time,
                    //setId: setId,
                    //Description: Description

                };
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
                    $scope.setId = Id;
                    $scope.SetName = Set_name;
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
                $scope.complete_set = function (e,string) {
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
             
             
                $scope.save = function () {
                    $scope.Scene.setId = parseInt($scope.setId );
                    $scope.Scene.environment_id = parseInt($scope.Scene.environment_id);
                    $scope.Scene.unit = unitid;
                    //$scope.Scene.scheduled_hh = $scope.Scene.scheduled_hh;
                    //$scope.Scene.scheduled_mm = $scope.Scene.scheduled_mm;
                    //$scope.Scene.Estime_ss = $scope.Scene.Estime_ss.toString();
                    //$scope.Scene.Estime_mm = $scope.Scene.Estime_mm.toString();

                    var characterId = [];
                    angular.forEach($scope.addedchar, function (item) {

                        characterId.push(item.Id.toString());

                    });
                    var extraId = [];
                    angular.forEach($scope.addedext, function (item) {

                        extraId.push(item.Id.toString());

                    });

                    var allscenes = {
                        "ExtraId": (extraId),
                        "CharacterId":(characterId),
                        "Id": 0,
                        "scene": $scope.Scene


                    }
                    console.log(JSON.stringify(allscenes));
                    console.log((allscenes));
                    $http.post(root + 'api/ScenesandScript/CreateScene/', (allscenes)
                    ).then(response => {
                        if (response.status == 200) {
                            toaster.pop({
                                type: 'success',
                                title: 'Success',
                                body: 'Scene created successfully!',
                            });
                            if ($scope.uploadedFiles) {
                                if ($scope.uploadedFiles.length > 0) {

                                    console.log(response);

                                    angular.forEach($scope.uploadedFiles, function (item) {

                                        item.SceneId = response.data.sceneId;

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
                            document.getElementById("ShootingInfo").reset();
                            document.getElementById("SceneForm").reset();
                            $rootScope.getscenes();
                        
                            $scope.cancel();
                        }
                    }, err => {
                            toaster.pop({
                                type: 'error',
                                title: 'Error',
                                body: err.data,
                            });
                    })
                }
              
        }, 100);
        });
 

});

//#region end Confiatech CreateScene Controller