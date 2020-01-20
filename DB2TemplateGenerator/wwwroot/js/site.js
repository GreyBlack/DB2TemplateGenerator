var vm = new Vue({
    el: "#app",
    //router: new VueRouter({ routes: routes }),
    data: {
        currnetDbType: "oracle",
        dbTypeList: ["oracle", "mysql", "sqlserver"],
        conn: "data source=192.168.126.99:1521/pkidb;user id=medicaluser;password=medicaluser;Incr Pool Size=5;Decr Pool Size=2;",
        currentTemplate: { code: "", type: "", name: "", content: "" },
        templateList: [
            { code: "oralce-sql-insert", type: "sql", name: "oracle insert语句" },
            { code: "xml-mapper", type: "xml", name: "MyBitis Mapper映射" },
            { code: "cs-entity", type: "cs", name: "C# 实体类" },
            { code: "po-entity", type: "cs", name: "C# 实体类[PO版本]" },
        ],
        tableName: "",
        resultContent: ""
    },
    methods: {
        chooseTemplate: function (template) {
            this.currentTemplate = template;
            this.currentTemplate.content = $("#" + template.code).text();
        },
        initParams: function () {
            var vm = this;
            return {
                conn: vm.conn,
                dbType: vm.currnetDbType,
                tableName: vm.tableName,
                fileType: vm.currentTemplate.type,
                template: vm.currentTemplate.content
            }
        },
        checkValid: function () {
            var vm = this;
            if (!vm.conn) { alert("连接字符串不能为空"); return false }
            if (!vm.currentTemplate.content) { alert("模板内容不能为空"); return false }
            return true
        },
        generateContent: function () {
            var vm = this;
            if (vm.checkValid()) {
                if (!vm.tableName) { alert("目标表名称不能为空"); return }
                axios.post("/generate/content", vm.initParams()).then(function (res) {
                    vm.resultContent = res.data.data
                }, function (res) {
                    alert(res.response.data.message)
                })
            }
        },
        generateFile: function () {
            var vm = this;
            if (vm.checkValid()) {
                if (vm.tableName || (!vm.tableName && confirm("未指定表名称，确认导出所有表文件？"))) {
                    axios({
                        method: "post",
                        url: "/generate/file",
                        data: vm.initParams(),
                        responseType: "blob"
                    }).then(function (res) {
                        var fileName = res.headers["content-disposition"].split("filename=")[1].split(";")[0]
                        var url = window.URL.createObjectURL(new Blob([res.data]));
                        vm.download(fileName, url)
                    }, function (res) {
                        var reader = new FileReader();
                        reader.onload = function (event) {
                            alert(JSON.parse(reader.result).message)
                        };
                        reader.readAsText(res.response.data);
                    });
                }
            }
        },
        download: function (fileName, url) {
            var link = document.createElement("a");
            link.style.display = "none";
            link.href = url;
            link.setAttribute("download", fileName);
            document.body.appendChild(link);
            link.click()
        }
    },
    created: function () { },
    watch: {
    }
})