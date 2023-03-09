﻿var viewPersonalModal = document.getElementById("personal-profile");

function ViewPersonalDetail() {
    $.ajax({
        url: window.location.origin + '/personal/profile',
        type: 'GET',
        success: function (user) {
            viewPersonalModal.style.display = "block";
            $(".profile-email").text(user.email);
            $(".profile-role").text(user.role);
            $(".profile-gender").text(user.gender);
            $(".profile-phonenumber").text(user.phonenumber);
            $(".profile-username").text(user.userName);
            $(".profile-department").text(user.department);

        }
    });
}

function ClosePopupProfile() {
    viewPersonalModal.style.display = "none";
}


//

//Popup Edit idea
var editIdeaModal = document.getElementById("editIdeaModal");
var editIdeaSpan = document.getElementsByClassName("close")[1];


function EditIdeaInfo() {
    var ideaId = $("#info-edit-ideaId").val();
    var myObject = {
        Title: $("#info-edit-title").val(),
        Content: $("#info-edit-content").val(),
        IsAnonymous: $("#isAnonymous").is(":checked"),
        CategoryId: parseInt($("#category_list").val()),
    };
    $.ajax({
        url: window.location.origin + '/personal/editIdea/' + ideaId,
        type: 'PUT',
        data: JSON.stringify(myObject),
        contentType: 'application/json',
        success: function () {
            alert('Edit idea successfully');
            window.location.reload();
        }
    });
}

function ViewIdeaDetail(id) {
    $.ajax({
        url: window.location.origin + '/personal/edit/' + id,
        type: 'GET',
        success: function (idea) {
            editIdeaModal.style.display = "block";
            getCategoriesForEditIdea(idea.categoryId);
            $("#info-edit-ideaId").val(idea.id);
            $("#info-edit-title").val(idea.title);
            $("#info-edit-content").val(idea.content);
            $("#isAnonymous").prop('checked', idea.isAnonymous);
        }
    });
}

function getCategoriesForEditIdea(categoryId) {
    $('#category_list option:not(:first)').remove();
    $.ajax({
        url: window.location.origin + '/idea/categories-selection',
        type: "GET",
        dataType: "JSON",
        data: "",
        success: function (categories) {
            $.each(categories, function (i, category) {
                $("#category_list").append(
                    $('<option></option>').val(category.id).html(category.name).prop("selected", category.id == categoryId)
                )
            });
        }
    })
}

function ClosePopupEditIdea() {
    editIdeaModal.style.display = "none";
}


//Soft Delete Idea
function SoftDeleteIdea(id) {
    var confirmResult = confirm("Are you sure you want to permanently delete this idea?");
    if (!confirmResult)
        return;

    $.ajax({
        url: window.location.origin + '/personal/softdeleteidea/' + id,
        type: 'PUT',
        success: function () {
            window.location.reload();
        },
    });
}

//Toggle deactive Idea
function ToggleDeactiveIdea(id) {
    var confirmResult = confirm("Are you sure?");
    if (!confirmResult)
        return;

    $.ajax({
        url: window.location.origin + '/personal/togglesdeactiveidea/' + id,
        type: 'PUT',
        success: function () {
            window.location.reload();
        },
    });
}

