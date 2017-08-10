$(document).ready(function () {
    /*if (window.carouselRotation) {
    window.clearInterval(window.carouselRotation);
    }
    var rotationInterval = 5000;
    var current_id = 1;
    var isRotating = true;
    var totalCarousel = $('#carouselWrapper .carouselItem').length;
    if (totalCarousel > 1) {
    window.carouselRotation = setInterval(
    function () {
    if (isRotating) {
    if (current_id == totalCarousel) {
    $('#carousel' + (current_id - 1)).css('left', '100%');
    $('#carousel' + current_id).animate({ left: '-100%' }, 500, null, function () {
    //$('#carousel' + current_id).css('display', 'none');
    });
    //$('#carousel1').css('display', 'block');
    $('#carousel1').animate({ left: '0px' }, 500);
    current_id = 1;
    }
    else if (current_id == 1) {
    $('#carousel' + totalCarousel).css('left', '100%');
    $('#carousel' + current_id).animate({ left: '-100%' }, 500, null, function () {
    //$('#carousel' + current_id).css('display', 'none');
    });
    //$('#carousel' + (current_id + 1)).css('display', 'block');
    $('#carousel' + (current_id + 1)).animate({ left: '0px' }, 500);
    current_id++;
    }
    else {
    $('#carousel' + (current_id - 1)).css('left', '100%');
    $('#carousel' + current_id).animate({ left: '-100%' }, 500, null, function () {
    //$('#carousel' + current_id).css('display', 'none');
    });
    //$('#carousel' + (current_id + 1)).css('display', 'block');
    $('#carousel' + (current_id + 1)).animate({ left: '0px' }, 500);
    current_id++;
    }
    $('#carouselWrapper .nodes .node.selected').removeClass('selected');
    $('#carouselWrapper .nodes .node[data-id=' + current_id + ']').addClass('selected');
    }
    }, rotationInterval
    );
    $('#carouselWrapper').hover(function () { isRotating = false; }, function () { isRotating = true; });
    $('#carouselWrapper .nodes .node').unbind('click').bind('click', function () {
    initCarousel($(this).attr('data-id'));
    });
    }
    */

    var rotationInterval = 7000,
        animationTime = 500,
        current_id = 1,
        isRotating = true,
        totalCarousel = $('#carouselWrapper .carouselItem').length,
        slideWidth = '100%';
    initCarousel();
    function initCarousel(selectedId) {
        if (window.carouselRotation) {
            window.clearInterval(window.carouselRotation);
        } 
        
        $('#carouselWrapper .carouselItem').finish();

        if (selectedId) {
            selectedId = Number(selectedId);
            if (selectedId != current_id) {
                var visibleItemId = current_id;
                var $selectedItem = $('#carouselWrapper .carouselItem[data-id=' + selectedId + ']');
                var $visibleItem = $('#carouselWrapper .carouselItem[data-id=' + visibleItemId + ']');
                if (visibleItemId > selectedId) {
                    $selectedItem.css('left', '-' + slideWidth);
                    $visibleItem.animate({ left: slideWidth }, animationTime);
                    $selectedItem.animate({ left: '0px' }, animationTime);
                }
                else {
                    $selectedItem.css('left', slideWidth);
                    $visibleItem.animate({ left: '-' + slideWidth }, animationTime);
                    $selectedItem.animate({ left: '0px' }, animationTime);
                }

                $('#carouselWrapper .nodes .node.selected').removeClass('selected');
                $('#carouselWrapper .nodes .node[data-id=' + current_id + ']').addClass('selected');
            }
        }
        else {
            selectedId = 1;
        }
        current_id = selectedId;

        $('#carouselWrapper .nodes .selected').removeClass('selected');
        $('#carouselWrapper .nodes .node[data-id=' + current_id + ']').addClass('selected');
        $('#carousel[data-id=' + current_id + ']').css('left', '0px');

        if (totalCarousel > 1) {
            window.carouselRotation = setInterval(
	            function () {
	                if (isRotating) {
	                    var preId = (current_id == 1) ? totalCarousel : (current_id - 1);
	                    var nextId = (current_id == totalCarousel) ? 1 : (current_id + 1);
	                    var $currentItem = $('#carouselWrapper .carouselItem[data-id=' + current_id + ']');
	                    var $preItem = $('#carouselWrapper .carouselItem[data-id=' + preId + ']');
	                    var $nextItem = $('#carouselWrapper .carouselItem[data-id=' + nextId + ']');

	                    $nextItem.css('left', slideWidth);
	                    $currentItem.animate({ left: '-' + slideWidth }, 500);
	                    $nextItem.animate({ left: '0px' }, 500);
	                    current_id = nextId;

	                    $('#carouselWrapper .nodes .selected').removeClass('selected');
	                    $('#carouselWrapper .nodes .node[data-id=' + current_id + ']').addClass('selected');
	                }
	            }, rotationInterval
            );
            $('#carouselWrapper').hover(function () { isRotating = false; }, function () { isRotating = true; });
            $('#carouselWrapper .nodes .node').unbind('click').bind('click', function () {
                initCarousel($(this).attr('data-id'));
            });
        }
    }

});