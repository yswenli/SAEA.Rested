$(function () {

    //初始化

    $("#dataTxt").hide();

    $("#methodSel").change(function () {
        if ($("#methodSel").val() === "POST") {
            $("#dataTxt").show();
        }
        else {
            $("#dataTxt").hide();
        }
    });

    $.get("/home/getlist", "", function (data) {
        $("#accordion").html(data);
    });

    //go按钮
    $("#runBtn").click(function () {

        var begin = (new Date()).getTime();

        $('#loading').modal('show');

        $("#alert-words").html("");

        var method = $("#methodSel").val();
        var url = $("#urlTxt").val();
        var data = $("#dataTxt").val();

        var jsonData = { "method": method, "url": encodeURIComponent(url), "data": encodeURIComponent(data) };        

        $.ajax({
            type: "POST",
            url: "/home/request",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(jsonData),
            dataType: "json",
            success: function (message) {

                var now = (new Date()).getTime() - begin;
                $("#alert-words").html("本次请求用时：" + now / 1000 + "秒");

                $("#reTxt").val(message.responseText);
                $('#loading').modal('hide');
            },
            error: function (message) {
                
                var now = (new Date()).getTime() - begin;
                $("#alert-words").html("本次请求用时：" + now/1000 + "秒");

                $("#reTxt").val(message.responseText);
                $('#loading').modal('hide');
            }
        });
    });

    //回车
    $("#urlTxt").keyup(function (keys) {
        if (keys.keyCode === 13) {
            $("#runBtn").click();
        }
    });

    //添加组
    $("#addGroupBtn").click(function () {

        var groupName = $("#addGroupNameTxt").val();

        if (groupName !== "") {

            $.post("/home/addgroup", "groupName=" + encodeURIComponent(groupName), function (data) {

                if (data === "1") {

                    $.get("/home/getlist", "", function (data) {

                        $("#accordion").html(data);
                    });

                    $("#addModal").modal('hide');
                }
            });
        }
    });

    //编辑组
    $("#accordion").on("click", ".group-edit", function () {
        $(this).parent().next().css("visibility", "visible");
        $(this).parent().hide();
    });

    $("#accordion").on("click", ".edit-group-btn", function () {
        var groupID = $(this).attr("data-id");
        var groupName = $(this).parent().prev().val();

        $.post("/home/updategroup", "groupID=" + groupID + "&groupName=" + encodeURIComponent(groupName), function (data) {

            if (data === "1") {

                $.get("/home/getlist", "", function (data) {

                    $("#accordion").html(data);
                });

                $("#addModal").modal('hide');
            }
        });
    });


    //保存按钮
    $("#saveBtn").click(function () {


        var method = $("#methodSel").val();
        var url = $("#urlTxt").val();
        var json = $("#dataTxt").val();

        if (url === "") {
            alert("Url地址不能为空！");
            return;
        }

        $("#saveBtn").attr("disabled", "disabled");

        var groupID = "";

        try {

            groupID = $(".group-title-lable").eq(0).attr("data-id");

            $(".group-title-lable").each(function (index) {
                if ($(this).attr("aria-expanded") === "true") {
                    groupID = $(this).attr("data-id");
                }
            });
        }
        catch{
            groupID = "";
        }

        if (groupID !== "") {
            $.post("/home/additem", `groupID=${groupID}&method=${method}&url=${encodeURIComponent(url)}&json=${encodeURIComponent(json)}`, function (data) {
                $.get("/home/getlist", "groupID=" + groupID, function (data) {
                    $("#accordion").html(data);
                    $("#saveBtn").removeAttr("disabled");
                });
            });
        }


    });

    //删除组
    $("#accordion").on("click", ".group-delete", function () {
        $("#deletegroupidTxt").val($(this).attr("data-id"));
    });

    $("#deletegroupBtn").click(function () {

        var groupID = $("#deletegroupidTxt").val();

        if (groupID !== "") {

            $.post("/home/removegroup", `groupID=${groupID}`, function (data) {

                $.get("/home/getlist", "", function (data) {
                    $("#accordion").html(data);
                });

                $('#deleteModal').modal('hide');
            });
        }
    });

    //删除项
    $("#accordion").on("click", ".delteItem", function () {
        $("#deleteitemidTxt").val($(this).attr("data-id"));
        $("#deleteitemgroupidTxt").val($(this).attr("data-group"));
    });

    $("#deleteitemBtn").click(function () {

        var groupID = $("#deleteitemgroupidTxt").val();

        var itemID = $("#deleteitemidTxt").val();

        if (groupID !== "" && itemID !== "") {

            $.post("/home/removeitem", `groupID=${groupID}&itemID=${itemID}`, function (data) {

                $.get("/home/getlist", "", function (data) {
                    $("#accordion").html(data);
                });

                $('#deleteItemModal').modal('hide');
            });
        }
    });

    //列表点击
    $("#accordion").on("click", ".urlLink", function () {

        var groupID = $(this).attr("data-group");

        var itemID = $(this).attr("data-id");

        if (groupID !== "" && itemID !== "") {

            $.get("/home/getitem", `groupID=${groupID}&itemID=${itemID}`, function (data) {
                if (data !== undefined || data !== "") {
                    $("#methodSel").val(data.Method);
                    $("#methodSel").change();
                    $("#urlTxt").val(data.Url);
                    $("#dataTxt").val(data.RequestJson);
                }
            });
        }

    });

    //导入
    $("#importDataBtn").click(function () {

        var json = $("#importDataTxt").val();

        if (json !== "") {
            $.post("/home/import", "json=" + encodeURIComponent(json), function (data) {
                if (data === "1") {
                    $.get("/home/getlist", "", function (data) {
                        $("#accordion").html(data);
                    });
                }
                else {
                    alert("数据导入失败！");
                }

                $("#importModal").modal('hide');
            });
        }
        else {
            alert("数据导入失败！");
            $("#importModal").modal('hide');
        }
    });

    //导出
    $("#exportBtn").click(function () {
        $.get("/home/export", null, function (data) {
            $("#exportDiv").html(JSON.stringify(data));
        });
    });

    //快捷键
    $(window).keydown(function (event) {
        if (!(event.ctrlKey && event.keyCode === 83)) return true;
        event.preventDefault();
        $("#saveBtn").click();
        return false;
    });
});
