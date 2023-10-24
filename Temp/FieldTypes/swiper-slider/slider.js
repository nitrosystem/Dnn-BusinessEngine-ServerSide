var touchThumbs = false;
var bField_SwiperSlider = function ($scope, field) {
    this.init = function () {
    }

    var layoutTemplate;
    var itemTemplate;
    var swiperDefaultOptions = {
        watchSlidesProgress: true,
        slidesPerView: 'auto',
        centeredSlides: false,
        loop: true,
        autoplay: false,
        parallax: false,
        speed: 300,
        mousewheel: true,
        simulateTouch: true,
        allowTouchMove: true,
        autoHeight: false,
        effect: "slide",
        navigation: {
            nextEl: '.swiper-button-next',
            prevEl: '.swiper-button-prev',
        },
        buttonOffset: [0, -22],
        paginationOffset: [0, 0],
        itemFirstsize: 90,
        pagination: {
            el: '.swiper-pagination',
            clickable: true,
        },
        direction: "horizontal",
        roundLengths: true,
        spaceBetween: 0,
        displaynumber: false,
        breakpoints: {
            480: {
                spaceBetween: 0
            },
            768: {
                spaceBetween: 0
            },
            992: {
                spaceBetween: 0
            },
            1200: {
                spaceBetween: 0
            },
            1600: {
                spaceBetween: 0
            }
        }
    };
    field.SliderOptions = swiperDefaultOptions;
    var reachEnd = false;
    var swiper_effect = {
        leftzoomout: {
            effect: 'leftzoomout',
            centeredSlides: false,
            on: {
                progress: function () {
                    var b, c, d, e, f, g;
                    var a = this;
                    var size = a.slidesSizesGrid;
                    var zindex = 100;
                    var opacity = 0;
                    for (b = 0; b < a.slides.length; b++) {
                        c = a.slides.eq(b);
                        d = c[0].progress;
                        style = c[0].style;
                        if (d > 0) {
                            e = .9 * d * size[b];
                            scale = 1 - .2 * d;
                            rotateY = 90 * d;
                            zindex = 100 * d;
                            opacity = 1 - d;
                            if (d > 1) {
                                scale = .8;
                                rotateY = 90;
                                zindex = 0;
                                opacity = 0;
                            };
                        } else {
                            e = 0;
                            scale = 1;
                            rotateY = 0;
                            zindex = 100;
                            opacity = 1;
                        }
                        style.opacity = opacity;
                        style.zIndex = zindex;
                        c.transform("translate3d(" + e + "px,0,-" + e + "px) scale(" + scale + ")  rotateY(" + rotateY + "deg)");
                    }
                },
                setTransition: function (b) {
                    var c, d, e;
                    var a = this;
                    for (c = 0; c < a.slides.length; c++) {
                        slide = a.slides.eq(c)
                        slide.transition(b);
                    }
                },
                slideChangeStart: function () {
                    var a = this;
                    if (a.autoplaying) {
                        a.bullets.eq(a.realIndex - 1).addClass("replace");
                        a.bullets.eq(a.realIndex - 1).removeClass("current firsrCurrent");
                        a.bullets.eq(a.realIndex).addClass("current");
                        if (0 == a.realIndex) {
                            a.bullets.removeClass("replace");
                        }
                    }
                },
                autoplayStop: function () {
                    var a = this;
                    a.$(".autoplay").removeClass("autoplay")
                },
                transitionEnd: function () {
                    this.slides.find('.swiper_animate_visible').removeClass('animated');
                    this.slides.eq(this.activeIndex).find('.swiper_animate_visible').addClass('animated');
                },
                init: function () {
                    this.slides.find('.swiper_animate_visible').removeClass('animated');
                    this.slides.eq(this.activeIndex).find('.swiper_animate_visible').addClass('animated');
                },
                touchMove: function (event) { },
                touchStart: function (event) {
                    reachEnd = false;
                },
                reachEnd: function () {
                    reachEnd = true;
                },
                touchEnd: function (event) {
                    var e = this;
                    setTimeout(function () {
                        if (reachEnd) {
                            e.slideTo(e.activeIndex - 1);
                            e.slideTo(e.activeIndex + 1);
                        }
                    }, 10)
                }
            }
        },
        centeredslides3d: {
            effect: 'centeredslides3d',
            centeredSlides: true,
            slidesPerView: 3,
            breakpoints: {
                480: {
                    slidesPerView: 1,
                    spaceBetween: 0
                },
                768: {
                    slidesPerView: 2,
                    spaceBetween: 0
                },
                992: {
                    slidesPerView: 3,
                    spaceBetween: 0
                },
                1200: {
                    slidesPerView: 3,
                    spaceBetween: 0
                },
                1600: {
                    slidesPerView: 3,
                    spaceBetween: 0
                }
            },
            on: {
                progress: function (progress) {
                    var cut = this.$el.data("number") == "3" ? 1 : 2;
                    var slidesSizesGrid = this.slidesSizesGrid[0] / 3;
                    var modify = 1;
                    for (i = 0; i < this.slides.length; i++) {
                        var slide = this.slides.eq(i);
                        var slideProgress = this.slides[i].progress;
                        modify = (Math.abs(slideProgress) - 1) * 0.3 + 1;
                        translate = slideProgress * modify * slidesSizesGrid + 'px';
                        scale = 1 - Math.abs(slideProgress) / 5;
                        zIndex = 999 - Math.abs(Math.round(10 * slideProgress));
                        slide.transform('translateX(' + translate + ') scale(' + scale + ')');
                        slide.css('zIndex', zIndex);
                        slide.css('opacity', 1);
                        if (Math.abs(slideProgress) > cut) {
                            slide.css('opacity', 0);
                            if (Math.abs(slideProgress) < cut + 1) {
                                slide.css('opacity', 1 - (Math.abs(slideProgress) - cut));
                            }
                        }
                    }
                },
                setTransition: function (transition) {
                    for (var i = 0; i < this.slides.length; i++) {
                        var slide = this.slides.eq(i)
                        slide.transition(transition);
                    }
                }
            }
        },
        scaleup: {
            effect: 'scaleup',
            breakpoints: false,
            on: {
                progress: function (progress) {
                    var slidesSizesGrid = this.slidesSizesGrid[0];
                    var number = parseInt(this.$el.data("number"));
                    var lastzoom = this.$el.data("lastzoom") ? 1 - this.$el.data("lastzoom") : 0.3;
                    var depth = this.$el.data("depth") ? this.$el.data("depth") : 2;
                    var width = this.width - slidesSizesGrid;
                    for (i = 0; i < this.slides.length; i++) {
                        var slide = this.slides.eq(i);
                        var slideProgress = this.slides[i].progress;
                        var style = slide[0].style;
                        if (slideProgress > 1) {
                            slide.css('opacity', 0);
                        } else if (slideProgress >= 0 && slideProgress <= 1) {
                            e = slideProgress * slidesSizesGrid + 'px';
                            zindex = 1000;
                            opacity = 1 - slideProgress;
                            style.opacity = opacity;
                            style.zIndex = zindex;
                            slide.transform("translateX(0) scale(1)");
                        } else {
                            zIndex = 999 - Math.abs(Math.round(10 * slideProgress));
                            slide.css('opacity', 1);
                            if (Math.abs(slideProgress) > number) {
                                scale = 1 - dnnEaseOut(number, 0, lastzoom, number);
                                modify = (slidesSizesGrid * (1 - scale)) / depth + dnnEaseOut(number, 0, width, number);
                                translate = slideProgress * slidesSizesGrid + modify + 'px';
                                slide.css('opacity', 0);
                            } else {
                                scale = 1 - dnnEaseOut(Math.abs(slideProgress), 0, lastzoom, number);
                                modify = (slidesSizesGrid * (1 - scale)) / depth + dnnEaseOut(Math.abs(slideProgress), 0, width, number);
                                translate = slideProgress * slidesSizesGrid + modify + 'px';
                                if (Math.abs(slideProgress) > number - 1) {
                                    slide.css('opacity', 1 - Math.min((Math.abs(slideProgress) - number + 1), 1));
                                }
                            }
                            slide.transform('translateX(' + translate + ') scale(' + scale + ')');
                            slide.css('zIndex', zIndex);
                        }
                    }
                },
                setTransition: function (transition) {
                    for (var i = 0; i < this.slides.length; i++) {
                        var slide = this.slides.eq(i)
                        slide.transition(transition);
                    }
                }
            }
        },
        scaleleft: {
            effect: 'scaleleft',
            breakpoints: false,
            on: {
                progress: function (progress) {
                    var slidesSizesGrid = this.slidesSizesGrid[0];
                    var number = parseInt(this.$el.data("number"));
                    var lastzoom = this.$el.data("lastzoom") ? 1 - this.$el.data("lastzoom") : 0.3;
                    var lastrotate = 25;
                    var depth = this.$el.data("depth") ? this.$el.data("depth") : 1;
                    var width = this.width - slidesSizesGrid;
                    for (i = 0; i < this.slides.length; i++) {
                        var slide = this.slides.eq(i);
                        var slideProgress = this.slides[i].progress;
                        var style = slide[0].style;
                        if (slideProgress > 1) {
                            slide.css('opacity', 0);
                            slide.css('pointer-events', 'none');
                        } else if (slideProgress >= 0 && slideProgress <= 1) {
                            zIndex = 1000;
                            scale = 1;
                            modify = dnnEaseOut(Math.abs(slideProgress), 0, width - 100, number);
                            translate = slideProgress * slidesSizesGrid + modify + 'px';
                            rotate = dnnEaseOut(slideProgress, 1, -15, 1) - 1;
                            slide.transform('translateX(' + translate + ') scale(' + scale + ') rotate(' + rotate + 'deg)');
                            slide.css('zIndex', zIndex);
                            slide.css('opacity', 1 - slideProgress);
                            slide.css('pointer-events', '');
                        } else {
                            zIndex = 999 - Math.abs(Math.round(10 * slideProgress));
                            slide.css('opacity', 1);
                            if (Math.abs(slideProgress) > number) {
                                scale = 1 - dnnEaseOut(number, 0, lastzoom, number);
                                rotate = dnnEaseOut(number, 0, lastrotate, number) - 1;
                                modify = (slidesSizesGrid * (1 - scale)) / depth + dnnEaseOut(number, 0, width, number);
                                translate = slideProgress * slidesSizesGrid + modify + 'px';
                                slide.css('opacity', 0);
                                slide.css('pointer-events', 'none');
                            } else {
                                scale = 1 - dnnEaseOut(Math.abs(slideProgress), 0, lastzoom, number);
                                rotate = dnnEaseOut(Math.abs(slideProgress), 0, lastrotate, number) - 1;
                                modify = (slidesSizesGrid * (1 - scale)) / depth + dnnEaseOut(Math.abs(slideProgress), 0, width, number);
                                translate = slideProgress * slidesSizesGrid + modify + 'px';
                                if (Math.abs(slideProgress) > number - 1) {
                                    slide.css('opacity', 1 - Math.min((Math.abs(slideProgress) - number + 1), 1));
                                }
                                slide.css('pointer-events', '');
                            }
                            slide.transform('translateX(' + translate + ') scale(' + scale + ') rotate(' + rotate + 'deg)');
                            slide.css('zIndex', zIndex);
                        }
                    }
                },
                setTransition: function (transition) {
                    for (var i = 0; i < this.slides.length; i++) {
                        var slide = this.slides.eq(i)
                        slide.transition(transition);
                    }
                }
            }
        },
        fade: {
            effect: 'fade',
            centeredSlides: true,
            fadeEffect: {
                crossFade: true,
            },
            on: {
                touchStart: function () {
                    touchThumbs = false;
                },
                transitionStart: function () {
                    //var d = $(this.$el).siblings('.swiper-thumbnail-pagination');
                    var d = $(field.SliderOptions.thumbnailSwiper);
                    if (!touchThumbs && d.length && d[0].swiper) {
                        d[0].swiper.slideTo(this.realIndex + d[0].swiper.passedParams.slidesPerView)
                    }
                    this.slides.eq(this.activeIndex).find('.swiper_animate_visible').removeClass('animated');
                },
                transitionEnd: function () {
                    this.slides.eq(this.activeIndex).find('.swiper_animate_visible').addClass('animated');
                }
            }
        },
        cube: {
            effect: 'cube',
            centeredSlides: true,
            cubeEffect: {
                slideShadows: false,
                shadow: false,
                shadowOffset: 100,
                shadowScale: 0.6
            }
        },
        coverflow: {
            effect: 'coverflow',
            centeredSlides: true,
            coverflowEffect: {
                slideShadows: false
            }
        },
        flip: {
            effect: 'flip',
            centeredSlides: true
        },
        animate: {
            effect: 'slide',
            on: {
                init: function () {
                    this.slides.find('.swiper_animate_visible').removeClass('animated');
                    this.slides.eq(this.activeIndex).find('.swiper_animate_visible').addClass('animated');
                },
                transitionEnd: function () {
                    this.slides.find('.swiper_animate_visible').removeClass('animated');
                    this.slides.eq(this.activeIndex).find('.swiper_animate_visible').addClass('animated');
                }
            }
        },
        testimonials02: {
            effect: 'fade',
            fadeEffect: {
                crossFade: true,
            },
            slidesPerView: 1,
            loop: true,
            pagination: false,
            simulateTouch: false,
            on: {
                touchStart: function () {
                    touchThumbs = false;
                },
                transitionStart: function () {
                    var d = $(this.$el).siblings('.swiper-thumbnail-pagination');
                    if (!touchThumbs && d.length && d[0].swiper) {
                        d[0].swiper.slideTo(this.realIndex + d[0].swiper.passedParams.slidesPerView)
                    }
                },
                init: function () {
                    this.slides.find('.swiper_animate_visible').removeClass('animated');
                    this.slides.eq(this.activeIndex).find('.swiper_animate_visible').addClass('animated');
                },
                transitionEnd: function () {
                    this.slides.find('.swiper_animate_visible').removeClass('animated');
                    this.slides.eq(this.activeIndex).find('.swiper_animate_visible').addClass('animated');
                },
            }
        },
        thumbnail: {
            loop: true,
            centeredSlides: true,
            slideToClickedSlide: true,
            pagination: false,
            on: {
                touchStart: function () {
                    touchThumbs = true;
                },
                transitionStart: function () {
                    //var d = $(this.$el).siblings('.swiper-container-main');
                    var d = $(field.SliderOptions.sliderSwiper);

                    if (touchThumbs && d.length && d[0].swiper) {
                        d[0].swiper.slideTo(this.realIndex + d[0].swiper.passedParams.slidesPerView)
                    }
                },
            }
        },
        inspiration: {
            direction: "vertical",
            effect: 'slide',
            slidesperview: "1",
            mousewheel: "false",
            observer: true,
            observeParents: true,
            on: {
                init: function () {
                    var that = $(this.el.parentElement);
                    that.append('<div class="swiper-content"></div>');
                    var content = that.find('.swiper-content');
                    var contentHtml = "";
                    that.find('.swiper-slide-item .content').each(function (index) {
                        contentHtml += '<div class="content">';
                        contentHtml += $(this).html();
                        contentHtml += '</div>';
                        $(this).remove();
                    });
                    content.append(contentHtml);
                    that.find(".swiper-content .content").removeClass("show");
                    that.find(".swiper-content .content").eq(this.activeIndex).addClass("show");
                },
                transitionEnd: function () {
                    var that = $(this.el.parentElement);
                    that.find(".swiper-content .content").removeClass("active");
                    that.find(".swiper-content .content").removeClass("show");
                    that.find(".swiper-content .content").eq(this.activeIndex).addClass("show");
                }
            }
        }
    };

    if (typeof field.Settings.SliderOptions == 'string') {
        field.Settings.SliderOptions = JSON.parse(field.Settings.SliderOptions.substr(1, field.Settings.SliderOptions.length - 2));

        field.SliderOptions = angular.extend({}, swiperDefaultOptions, field.Settings.SliderOptions);

        if (field.Settings.SliderOptions.effect && swiper_effect[field.Settings.SliderOptions.effect]) {
            field.SliderOptions = angular.extend({}, field.SliderOptions, swiper_effect[field.Settings.SliderOptions.effect]);
        }
    }

    if (field.Settings.LayoutTemplate) {
        layoutTemplate = field.Settings.LayoutTemplate;
        layoutTemplate = layoutTemplate.replace('[SLIDERCSSCLASS]', (field.Settings.SliderCssClass ? field.Settings.SliderCssClass : ''));
    }

    if (field.Settings.ItemTemplate) {
        itemTemplate = field.Settings.ItemTemplate;
    }

    if (field.Settings.DataSource) {
        $.grep($scope.actions, function (a) { return a.ActionID == field.Settings.DataSource.ActionID }).map(function (action) {
            listName = /{(\w+)}$/.exec(action.ActionDetails.ResultName)[1];
        });
    }

    $scope.$watch('Field.' + field.FieldName + '.Options', function (newVal, oldVal) {
        if (newVal != oldVal && newVal.length) {
            if (field.Settings.CustomCss) $('#swiper' + field.FieldName).html('<style type="text/css">' + field.Settings.CustomCss + '</style> \n ');

            $('#swiper' + field.FieldName).append(layoutTemplate);

            $('#swiper' + field.FieldName + ' .swiper-container').css('direction', 'ltr');

            for (var i = 0; i < newVal.length; i++) {
                var template = itemTemplate.replace(/{Item\.(.[^}]+)}/gm, '{' + listName + '[' + i + '].$1}');
                var $sliderItem = $('<div class="swiper-slide"></div>');
                $sliderItem.append($scope.$compile(template)($scope));
                $('#swiper' + field.FieldName + ' .swiper-wrapper').append($sliderItem);
            }

            setTimeout(function () {
                new Swiper('#swiper' + field.FieldName + ' .swiper-container', field.SliderOptions);

                $('#swiper' + field.FieldName + ' .swiper-container').children(".swiper-wrapper").wrap("<div class=\"swiper-wrapper-overflow\">");
            });
        }
    });
}