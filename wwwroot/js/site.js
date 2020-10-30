// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('.myTables').DataTable();
});

function ToggleEdit(id) {
    var e = document.getElementById("editForm-" + id);
    if (e.style.display == 'none') {
        document.getElementById("comment-" + id).style.display = 'none';
        e.style.display = 'block';
    } else {
        document.getElementById("comment-" + id).style.display = 'block';
        e.style.display = 'none';
    }
}


function CommentEditor(id, created, comment) {
    console.log(id, created, comment);
    // where to remove/place children
    var div = document.getElementById("comment-" + id);

    // child to remove
    comment = comment.firstChild.nextElementSibling;

    if (comment.tagName == 'P') {
        comment.remove();

        // form
        var form = document.createElement("FORM");
        form.setAttribute("method", "post");
        form.setAttribute("action", "/Comments/Edit/" + id);

        // input content
        var input = document.createElement("INPUT")
        input.setAttribute("type", "text");
        input.setAttribute("name", "content");
        input.setAttribute("value", comment.innerText);

        // input content
        var input1 = document.createElement("INPUT")
        input1.setAttribute("type", "text");
        input1.setAttribute("name", "created");
        input1.setAttribute("value", created);

        // button
        var btn = document.createElement("BUTTON")
        var text = document.createTextNode("Submit");
        btn.appendChild(text);
        btn.setAttribute("type", "submit");
        btn.classList.add("btn");
        btn.classList.add("btn-primary");
        btn.classList.add("btn-sm");
        btn.classList.add("mb-1");

        // append input and btn to form
        form.appendChild(input);
        form.appendChild(btn);

        // append form to div
        div.appendChild(form);
    }
}
