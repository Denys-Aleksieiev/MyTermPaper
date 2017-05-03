$(document).ready(function () {
    var progBar = $('<div/>').addClass('progress upload-progress').append(
            $('<div/>').addClass('progress-bar').attr({
                'role': 'progressbar',
                'aria-valuenow': '0',
                'aria-valuemin': '0',
                'aria-valuemax': '100',
                'style': 'width: 0%;'
            })
        );

    $('#fileUploadButton').fileupload({
        dataType: 'json',
        autoUpload: true,
        sequentialUploads: true,
        url: '/MyStorage/UploadFiles',
        add: function (e, data) {
            var progressBar = progBar.clone();
            var progressCell = $('<td/>').html(progressBar);
            var row = $('<tr/>').append($('<td/>').text(data.files[0].name)).append(progressCell);
            data.context = progressCell;
            data.progressBar = progressBar;
            $('#uploadFilesList').append(row);
            data.submit();
        },
        progress: function (e, data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);
            data.progressBar.children().css('width', progress + '%');
        },
        done: function (e, data) {
            data.context.html('<div>lol</div>');
            if (data.result.status == "True") {
                data.context.html(
                    "<td><span class='glyphicon glyphicon-ok-circle green'></span> " + data.result.message + "</td> </tr>");
                UpdateFilesRequest();
                UpdateFreeStorageSizeRequest();
            }
            else {
                data.context.html(
                    "<td><span class='glyphicon glyphicon-remove-circle red'></span> " + data.result.message + "</td> </tr>");
            }
        },
        fail: function (e, data) {
            data.context.html(
                 "<td><span class='glyphicon glyphicon-remove-circle red'></span>This file is too big. </td> </tr>");
        }
    });

    $("#compressionSelect").change(function () {
        // This is the jQuery way of finding selected option's text
        var myVar = $(this).find("option:selected").val();
        // Post the value to your server. You should do some error handling here
        $.post("/MyStorage/SetCompression", {
            selectedCompression: myVar
        });
    });

    $('#documentFileUploadButton').fileupload({
        dataType: 'json',
        autoUpload: true,
        sequentialUploads: true,
        url: '/MyStorage/UploadFiles',
        add: function (e, data) {
            var progressBar = progBar.clone();
            var progressCell = $('<td/>').html(progressBar);
            var row = $('<tr/>').append($('<td/>').text(data.files[0].name)).append(progressCell);
            data.context = progressCell;
            data.progressBar = progressBar;
            $('#uploadFilesList').append(row);
            data.submit();
        },
        progress: function (e, data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);
            data.progressBar.children().css('width', progress + '%');
        },
        done: function (e, data) {
            data.context.html('<div>lol</div>');
            if (data.result.status == "True") {
                data.context.html(
                    "<td><span class='glyphicon glyphicon-ok-circle green'></span> " + data.result.message + "</td> </tr>");
                UpdateDocumentFilesRequest();
                UpdateFreeStorageSizeRequest();
            }
            else {
                data.context.html(
                    "<td><span class='glyphicon glyphicon-remove-circle red'></span> " + data.result.message + "</td> </tr>");
            }
        },
        fail: function (e, data) {
            data.context.html(
                 "<td><span class='glyphicon glyphicon-remove-circle red'></span>This file is too big.</td> </tr>");
        }
    });

    $('#audioFileUploadButton').fileupload({
        dataType: 'json',
        autoUpload: true,
        sequentialUploads: true,
        url: '/MyStorage/UploadFiles',
        add: function (e, data) {
            var progressBar = progBar.clone();
            var progressCell = $('<td/>').html(progressBar);
            var row = $('<tr/>').append($('<td/>').text(data.files[0].name)).append(progressCell);
            data.context = progressCell;
            data.progressBar = progressBar;
            $('#uploadFilesList').append(row);
            data.submit();
        },
        progress: function (e, data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);
            data.progressBar.children().css('width', progress + '%');
        },
        done: function (e, data) {
            data.context.html('<div>lol</div>');
            if (data.result.status == "True") {
                data.context.html(
                    "<td><span class='glyphicon glyphicon-ok-circle green'></span> " + data.result.message + "</td> </tr>");
                UpdateAudioFilesRequest();
                UpdateFreeStorageSizeRequest();
            }
            else {
                data.context.html(
                    "<td><span class='glyphicon glyphicon-remove-circle red'></span> " + data.result.message + "</td> </tr>");
            }
        },
        fail: function (e, data) {
            data.context.html(
                 "<td><span class='glyphicon glyphicon-remove-circle red'></span>This file is too big. </td> </tr>");
        }
    });

    $('#videoFileUploadButton').fileupload({
        dataType: 'json',
        autoUpload: true,
        sequentialUploads: true,
        url: '/MyStorage/UploadFiles',
        add: function (e, data) {
            var progressBar = progBar.clone();
            var progressCell = $('<td/>').html(progressBar);
            var row = $('<tr/>').append($('<td/>').text(data.files[0].name)).append(progressCell);
            data.context = progressCell;
            data.progressBar = progressBar;
            $('#uploadFilesList').append(row);
            data.submit();
        },
        progress: function (e, data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);
            data.progressBar.children().css('width', progress + '%');
        },
        done: function (e, data) {
            data.context.html('<div>lol</div>');
            if (data.result.status == "True") {
                data.context.html(
                    "<td><span class='glyphicon glyphicon-ok-circle green'></span> " + data.result.message + "</td> </tr>");
                UpdateVideoFilesRequest();
                UpdateFreeStorageSizeRequest();
            }
            else {
                data.context.html(
                    "<td><span class='glyphicon glyphicon-remove-circle red'></span> " + data.result.message + "</td> </tr>");
            }
        },
        fail: function (e, data) {
            data.context.html(
                 "<td><span class='glyphicon glyphicon-remove-circle red'></span>  This file is too big.  </td> </tr>");
        }
    });


    $("#deleteConfirmModal").on('click', "#deleteConfirm", function () {
        DeleteFileRequest();
        $('#deleteConfirmModal').modal('hide');
    });

    //Search
    $("#btnSearch").click(function () {
        UpdateFilesRequest();
    });
    $("#searchString").keypress(function (e) {
        if (e.keyCode == 13) {
            UpdateFilesRequest();
        }
    });

    //Caret and sorting control
    $('#columnNameHead').click(function (event) {
        var elemId = event.target.id;
        if (currenActiveSortId == elemId) {
            nameSort = !nameSort;
        }
        else {
            nameSort = true;
            dateSort = null;
            sizeSort = null;
            ChangeCaretState('columnDateHead', 'none');
            ChangeCaretState('columnSizeHead', 'none');
        }
        ChangeCaretState(elemId, nameSort);
        currenActiveSortId = elemId;
        UpdateFilesRequest();
    });
    $('#columnDateHead').click(function (event) {
        var elemId = event.target.id;
        if (currenActiveSortId == elemId) {
            dateSort = !dateSort;
        }
        else {
            nameSort = null;
            dateSort = true;
            sizeSort = null;
            ChangeCaretState('columnNameHead', 'none');
            ChangeCaretState('columnSizeHead', 'none');
        }
        ChangeCaretState(elemId, dateSort);
        currenActiveSortId = elemId;
        UpdateFilesRequest();
    });
    $('#columnSizeHead').click(function (event) {
        var elemId = event.target.id;
        if (currenActiveSortId == elemId) {
            sizeSort = !sizeSort;
        }
        else {
            nameSort = null;
            dateSort = null;
            sizeSort = true;
            ChangeCaretState('columnNameHead', 'none');
            ChangeCaretState('columnDateHead', 'none');
        }
        ChangeCaretState(elemId, sizeSort);
        currenActiveSortId = elemId;
        UpdateFilesRequest();
    });

    $('#accessByLinkSelect').selectpicker({
        style: 'btn-default'
    });

});

var currentFileId;

var currenActiveSortId = 'columnNameHead';
var nameSort = true;
var dateSort;
var sizeSort;

function ConfirmDelete(element) {
    $('#deleteConfirmModal').modal('show');
    currentFileId = $(element).parent().parent().prop("id");
}
function ShareFile(element) {
    var el = $(element);
    var id = el.parent().parent().prop('id');
    el.removeClass('btn-default');
    el.addClass('btn-success');
    GetShareLinkRequest(id);
    currentFileId = id;
    $('#fileSharingModal').modal('show');
}
function FileSharingDone(element) {
    var el = $(element);
    var access = $('#accessByLinkSelect').val();
    if (access == "accessOff") {
        $.ajax({
            type: "POST",
            url: "/MyStorage/DeleteShareLink",
            data: { fileId: currentFileId },
            success: function (data) {
                if (data.status == 'true') {
                    $('#accessByLinkSelect').selectpicker('deselectAll');
                    $('#accessByLinkSelect').selectpicker('val', 'accessOn');
                    var btn = $('#' + currentFileId).children(3).children('.btn-success');
                    btn.removeClass('btn-success');
                    btn.addClass('btn-default');
                }
                else {

                }
            }
        });
    }
}
function ChangeCaretState(id, caretState) {
    switch (caretState) {
        case true:
            $('#' + id + '> span').removeClass('dropup');
            $('#' + id + '> span > span').addClass('caret');
            break;
        case false:
            $('#' + id + '> span > span').addClass('caret');
            $('#' + id + '> span').addClass('dropup');
            break;
        case "none":
            $('#' + id + '> span').removeClass('dropup');
            $('#' + id + '> span > span').removeClass('caret');
            break;
    }
}
function UploadFilesModal() {
    $('#fileUploadModal').on('hidden.bs.modal', function (e) {
        $('#uploadFilesList').empty();

    });
    $('#fileUploadModal').modal('show');
}


function GetSortOrder() {
    if (nameSort != null) {
        return nameSort ? "Name" : "NameDesc";
    }
    if (dateSort != null) {
        return dateSort ? "Date" : "DateDesc";
    }
    if (sizeSort != null) {
        return sizeSort ? "Size" : "SizeDesc";
    }
}
function DeleteFileCallback(data) {
    //add bootstrap notify!! or another notification
    if (data.status == 'true') {
        UpdateFilesRequest();
        UpdateFreeStorageSizeRequest();
    }
}
function UpdateFilesCallback(data) {
    $('#files_table_body').html(data);
}


function GetShareLinkRequest(fileId) {
    $.ajax({
        type: "GET",
        url: "/MyStorage/ShareLink",
        data: {
            fileId: fileId,
        },
        success: function (data) {
            $('#shareLinkTextbox').val(data.shareLink);
        }
    });
}
function DeleteFileRequest() {
    $.ajax({
        type: "POST",
        url: "/MyStorage/Delete",
        data: {
            fileId: currentFileId
        },
        success: DeleteFileCallback
    });
}
function UpdateFilesRequest() {
    $.ajax({
        type: "GET",
        url: "/MyStorage/UserFiles",
        data: {
            sortOrder: GetSortOrder(),
            searchString: $('#searchString').val(),
        },
        success: UpdateFilesCallback
    });
}
function UpdateDocumentFilesRequest() {
    $.ajax({
        type: "GET",
            url: "/MyStorage/UserDocumentFiles",
                data: {
                sortOrder: GetSortOrder(),
                        searchString: $('#searchString').val(),
                        },
                            success : UpdateFilesCallback
                });
}
function UpdateAudioFilesRequest() {
    $.ajax({
        type: "GET",
            url: "/MyStorage/UserAudioFiles",
                data: {
                sortOrder: GetSortOrder(),
                        searchString: $('#searchString').val(),
                        },
                            success : UpdateFilesCallback
                });
}
function UpdateVideoFilesRequest() {
    $.ajax({
        type: "GET",
            url: "/MyStorage/UserVideoFiles",
                data: {
                sortOrder: GetSortOrder(),
                        searchString: $('#searchString').val(),
                        },
                            success : UpdateFilesCallback
                });
                }
function UpdateFreeStorageSizeRequest()
{
    $.ajax({
        type: "GET",
        url: "/MyStorage/UserStorageSize",
        success: function (data) {
            $('#storageCapacityText').html('Ocupied ' + data.occupiedSize + ' from ' + data.totalSize);
            $('#freeStorageSize').css('width', data.percent + '%');
        }
    });
}