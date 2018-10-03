
// GET NOTIFICATIONS AND SETUP SIGNALR

// Click on notification icon to show notification
$('span.noti').click(function (e) {
    e.stopPropagation();
    $('.noti-content').show();
    var count = 0;
    count = parseInt($('span.count').html()) || 0;
    // Only load notification if not already loaded
    if (count > 0) {
        updateNotification();
    }
    $('span.count', this).html('&nbsp;');
});

// Hide notifications
$('html').click(function () {
    $('.noti-content').hide();
});

// Update notifications
function updateNotification() {
    $('#notiContent').empty();
    $('#notiContent').append($('<li>Loading...</li>'));
    $.ajax({
        type: 'GET',
        url: '/home/GetNotificationContacts',
        success: function (response) {
            $('#notiContent').empty();
            if (response.length === 0) {
                $('#notiContent').append($('<li>No data available</li>'));
            }
            $.each(response, function (index, value) {
                $('#notiContent').append($('<li>New contact : ' + value.ContactName + ' (' + value.ContactNo + ') added</li>'));
            });
        },
        error: function (error) {
            console.log(error);
        }
    });
}

// Update notifications count
function updateNotificationCount() {
    var count = 0;
    count = parseInt($('span.count').html()) || 0;
    count++;
    $('span.count').html(count);
}

// SignalR js code for start hub and send receive notification(s) from the server side
var notificationHub = $.connection.notificationHub;
$.connection.hub.start().done(function () {
    console.log('Notification hub started');
});

// SignalR method for push server message to client
notificationHub.client.notify = function (message) {
    if (message && message.toLowerCase() === "added") {
        updateNotificationCount();
    }
};

