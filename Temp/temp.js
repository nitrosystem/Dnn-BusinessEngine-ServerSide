<div class="art-widget art-main-info">
    <div class="row mb-5">
        <div class="col-1">
            <div class="icon-actions">
                <ul class="icons">
                    <li>
                        <i class="bi bi-share fa-8x"></i>
                    </li>
                    <li>
                        <i class="bi bi-bookmark"></i>
                    </li>
                    <li class="with-label">
                        <i class="bi bi-heart"></i> 40
                    </li>
                    <li class="with-label">
                        <i class="bi bi-chat-square"></i> 18
                    </li>
                </ul>
            </div>
        </div>
        <div class="col-5 border-end art-border">
            <div class="art-title">
                <h1 class="title" bind-text="Art.Title"></h1>
            </div>
            <div class="subtitle">
                <!--------دسته بندی اثر نوشتاری-سنما-موسیقی------------
                ------------------------------------------------------------------------------------------->
                <div class="category-item" b-show="Art.CategoryOfArtWriting">
                    <p ng-repeat="cat in Art.CategoryOfArtWriting">
                        <a bind-url="/artists?c=cat.Title" class="hashtag" bind-text="cat.Title">
                        </a>
                    </p>
                </div>
                <div class="category-item" b-show="Art.ThematicCategory">
                    <p ng-repeat="cat in Art.ThematicCategory">
                        <a bind-url="/artists?c=cat.Title" class="hashtag" bind-text="cat.Title">
                        </a>
                    </p>
                </div>
                <div class="category-item" b-show="Art.ArtStructureFormats">
                    <p ng-repeat="struct in Art.ArtStructureFormats">
                        <a bind-url="/artists?s=struct.Title" class="hashtag" bind-text="struct.Title">
                        </a>
                    </p>
                </div>

                <!--------ژانر های گروه های  مختلف------------
                ------------------------------------------------------------------------------------------->
                <div class="category-item" b-show="Art.Artgeners">
                    <p ng-repeat="gener in Art.Artgeners">
                        <a bind-url="/artists?g=gener.Title" class="hashtag" bind-text="gener.Title">

                        </a>
                    </p>
                </div>

                <div class="category-item" b-show="Art.TypeOfMusicValue">
                    <p>
                        <a bind-url="/artists?tm=Art.TypeOfMusicValue" class="hashtag" bind-text="Art.TypeOfMusicValue">
                        </a>
                    </p>
                </div>
                <div class="category-item" b-show="Art.MusicDeviceValue">
                    <p>
                        <a bind-url="/artists?md=Art.MusicDeviceValue" class="hashtag" bind-text="Art.MusicDeviceValue">
                        </a>
                    </p>
                </div>
                <div class="category-item" b-show="Art.TimeFormatValue">
                    <p>
                        <a bind-url="/artists?tf=Art.TimeFormatValue" class="hashtag" bind-text="Art.TimeFormatValue">

                        </a>
                    </p>
                </div>

            </div>
            <div class="art-description">
                <p class="description" bind-text="Art.Summary"></p>
                <a href="#" class="more-button right">
                    <i class="bi bi-plus"></i> ادامه
                </a>
            </div>
            <div class="art-authors art-border">
                <label class="header-label">مولفین این اثر</label>
                <div class="authors">
                    <div class="author">
                        <div class="image-wrapper">
                            <img bind-image="Art.ArtistPhoto" no-image="/portals/0/no-image.png" />
                        </div>
                        <span class="author-name" bind-text="Art.ArtistName"></span>
                    </div>
                </div>
            </div>
            <div class="art-scores">
                <div class="score-panel">
                    <label class="header-label">امتیازات</label>
                    <div class="stars">
                        <div class="score-action">
                            <div style="direction:ltr">
                                <input class="rate-art" type="hidden" />
                            </div>
                            <p b-show="Form.UserRated" class="user-rated-message">
                                از امتیازدهی شما به این هنرمند سپاسگزاریم!.
                            </p>
                        </div>
                        <div class="score-result">
                            <span bind-text="Art.Rate"></span>
                            <i class="bi bi-star"></i>
                        </div>
                    </div>
                </div>
            </div>
            <div class="buttons">
                <button type="button" ng-click="bButton_onClick(Field.BuyButton,$event)" class="btn-azadi-1 w-100">
                    <i class=""></i>
                    خرید و بهره برداری قانونی از این اثر
                </button>
            </div>
        </div>
        <div class="col-4">
            <div class="art-image">
                <div class="image-wrapper">
                    <img bind-image="Art.Photo" no-image="/portals/0/no-image.png" />
                </div>
            </div>
        </div>
        <div class="col-2">
            <div class="art-awards art-border">
                <div class="btn-azadi-1 btn-sm mb-1">
                    جوایز ادبی
                </div>
                <div class="awards">
                    <div class="award">
                        <div class="image-wrapper">
                            <img src="/portals/0/award-1.jpg" />
                        </div>
                        <span class="award-name">جایزه ادبی استاد فارسی</span>
                    </div>
                    <div class="award">
                        <div class="image-wrapper">
                            <img src="/portals/0/award-2.jpg" />
                        </div>
                        <span class="award-name">جایزه ادبی قلم طلایی</span>
                    </div>
                </div>
                <a href="#" class="more-link">+ 5 جایزه این اثر</a>
            </div>
            <div class="art-events art-border">
                <div class="btn-azadi-1 btn-sm">
                    رویداد مرتبط
                </div>
                <ul class="events">
                    <li>جشن رونمایی از کتاب ویولون زن روی پل</li>
                    <li>یاداشت نویسنده درباره نحوه پرداخت اثر</li>
                    <li>نقد بررسی از این کتاب در شهر مشهد</li>
                    <li>یاداشت نویسنده درباره نحوه پرداخت اثر</li>
                </ul>
                <a href="#" class="more-link">مطالب بیشتر</a>
            </div>
            <div class="art-likes">
                <label class="header-label">این اثر را پسندیدند</label>
                <div class="peoples">
                    <div class="image-wrapper">
                        <img src="/portals/0/user-1.jpg" />
                    </div>
                    <div class="image-wrapper">
                        <img src="/portals/0/user-2.jpg" />
                    </div>
                    <div class="image-wrapper">
                        <img src="/portals/0/user-3.jpg" />
                    </div>
                    <div class="image-wrapper">
                        <img src="/portals/0/user-4.jpg" />
                    </div>
                    <div class="image-wrapper">
                        <img src="/portals/0/user-5.jpg" />
                    </div>
                </div>
                <a href="#" class="btn-azadi-2">مشاهده همه </a>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-12">
            <div class="art-certificate">
                <div class="certificate-wrapper">
                    <label class="certificate-label">شناسنامه اثر</label>
                    <div class="item">
                        <label>کشور پدیدآورنده اثر</label>
                        <span>
                            {{Art.Country}}
                        </span>
                    </div>
                    <div class="item" b-show="Art.Ages">
                        <label>رده سنی </label>
                        <span>
                            <span b-show="Art.Ages ==1 "> عمومی</span>
                            <span b-show="Art.Ages == 2"> کودک و نوجوان </span>
                            <span b-show="Art.Ages == 3"> بزرگسال </span>
                            <span b-show="Art.Ages == 9"> +9 </span>
                            <span b-show="Art.Ages == 12"> +12 </span>
                            <span b-show="Art.Ages == 15"> +15 </span>
                            <span b-show="Art.Ages == 18"> +18 </span>
                        </span>
                    </div>
                    <div class="item">
                        <label>زبان انتشار </label>
                        <span>
                            <span bind-text="Art.PublicationLanguage"></span>
                        </span>
                    </div>
                    <div class="item">
                        <label>سال انتشار اثر</label>
                        <span b-show="Art.PersianLanguage == 1" bind-text="Art.SolarYearFirstPublication"></span>
                        <span b-show="Art.PersianLanguage == 0" bind-text="Art.GregorianYearFirstPublication"></span>
                    </div>
                    <div class="item">
                        <label>پدیدآورندگان اصلی اثر</label>
                        <span bind-text="Art.ArtistName"></span>
                    </div>
                    <div class="item">
                        <label>ناشر اثر</label>
                        <!--اثر نوشتاری-->
                        <span b-show="Art.ArtType == 1" bind-text="Art.Publisher"> </span>
                        <!--اثر موسیقی-->
                        <span b-show="Art.ArtType == 2" bind-text="Art.AlbumPublisherName"> </span>
                        <!--اثر سینما-->
                        <span b-show="Art.ArtType == 3" bind-text="Art.Producer"> </span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="related-arts">
    <div class="related-weapper">
        <label class="related-label">آثــــار مشابه</label>
        <div class="items">
            <div class="image-wapper">
                <img src="/portals/0/related-1.jpg" />
            </div>
            <div class="image-wapper">
                <img src="/portals/0/related-2.jpg" />
            </div>
            <div class="image-wapper">
                <img src="/portals/0/related-3.jpg" />
            </div>
            <div class="image-wapper">
                <img src="/portals/0/related-4.jpg" />
            </div>
            <div class="image-wapper">
                <img src="/portals/0/related-1.jpg" />
            </div>
            <div class="image-wapper">
                <img src="/portals/0/related-2.jpg" />
            </div>
            <div class="image-wapper">
                <img src="/portals/0/related-3.jpg" />
            </div>
        </div>
    </div>
</div>
<div class="art-contents">
    <div class="row">
        <div class="col-7">
            <div class="art-widget">
                <ul class="nav nav-tabs" role="tablist">
                    <li class="nav-item" role="presentation">
                        <button class="nav-link active" id="review-tab" data-bs-toggle="tab" data-bs-target="#review" type="button" role="tab" aria-controls="review" aria-selected="true">مرور مطالب</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" id="jostar-tab" data-bs-toggle="tab" data-bs-target="#jostar" type="button" role="tab" aria-controls="jostar" aria-selected="false">نقد و جستار</button>
                    </li>
                    <li class="nav-item" role="presentation">
                        <button class="nav-link" id="news-tab" data-bs-toggle="tab" data-bs-target="#news" type="button" role="tab" aria-controls="news" aria-selected="false">اخبار روز</button>
                    </li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane fade show active" id="review" role="tabpanel" aria-labelledby="review-tab">
                        <a href="" class="link-icon-1" b-click="onShowReviewModal()">
                            <i class="bi bi-plus-square-fill"></i>
                            <span> اضافه کردن یک مطلب </span>
                        </a>
                        <hr />
                        <!------------------------------------>
                        <!--مودال ثبت ریویو-->
                        <!------------------------------------>
                        <div class="modal fade" id="wnSubmitReview" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-hidden="true">
                            <div class="modal-dialog modal-lg modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title">ثبت ریویو</h5>
                                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                    </div>
                                    <div class="modal-body submit-review">
                                        <div class="row" b-show="!Form.ReviewSubmited">
                                            <div class="col-7 border-end">
                                                <div class="mb-10">
                                                    <label class="form-label">امتیاز شما</label>
                                                    <div style="direction:ltr">
                                                        <input class="rate-art" type="hidden" />
                                                    </div>
                                                    <p b-show="Form.UserRated" class="user-rated-message">
                                                        از امتیازدهی شما به این هنرمند سپاسگزاریم!.
                                                    </p>
                                                </div>
                                                <div class="mb-1">
                                                    <label class="form-label">ریویو(بررسی) شما</label>
                                                    <input class="b-input form-control mb-10" type="text" ng-model="Form.Title" placeholder="عنوان ریویو مورد نظر خود را وارد نمایید">
                                                    <textarea class="b-input form-control" ng-model="Form.Review" rows="7" placeholder="لطفا ریویو مورد نظر خود را به صورت فارسی رسمی وارد نمایید."></textarea>
                                                    <span class="art-small-text" b-show="!Form.Review || Form.Review.length<600">حداقل کاراکتر مورد نیاز {{600 - Form.Review.length}} کاراکتر می باشد</span>
                                                    <span class="art-small-text text-danger d-block" b-show="Form.ReviewRequiredError">وارد نمودن این فیلد الزامی است.</span>
                                                </div>
                                            </div>
                                            <div class="col-5">
                                                <ol class="review-notes">
                                                    <li>
                                                        هر کاربر تنها یک بار می تواندثبت ریویو و امتیاز دهی داشته باشد.
                                                    </li>
                                                    <li>
                                                        ریویو شما پس از تایید توسط واحد تحریریه منتظر خواهد شد.
                                                    </li>
                                                    <li>
                                                        حداقل تعداد کاراکتر برای ثبت ریویو 600 کاراکتر و حداکثر تعداد کاراکتر 10,000 کاراکتر می باشد.
                                                    </li>
                                                    <li>
                                                        ریویوهایی که غیرمرتبط به موضوع اثر باشند تایید نخواهند شد.
                                                    </li>
                                                    <li>
                                                        برای کسب اطلاعات بیشتر در مورد فرایند تثبت و تایید ریویو این صفحه را دنبال نمایید.
                                                    </li>
                                                </ol>
                                            </div>
                                        </div>
                                        <div b-show="Form.ReviewSubmited" class="alert alert-info text-center">
                                            <div class="h1 mb-15">
                                                <i class="bi bi-check2-square"></i>
                                            </div>
                                            با تشکر از شما بابت ثبت ریویو!.
                                            <br> ریویوی شما با موفقیت ثبت شد و در صف انتشار قرار گرفت.<br> چنانچه ریویو مغایر با قوانین سایت نباشد در اولین فرصت تایید خواهد شد.<br>
                                            <button type="button" class="btn btn-info mt-10" b-click="onCloseReviewModalClick()">بستن</button>
                                        </div>
                                    </div>
                                    <div class="modal-footer" b-show="!Form.ReviewSubmited">
                                        <button type="button" class="btn btn-primary ms-2" b-click="onSubmitReviewClick()">ثبت ریویو</button>
                                        <button type="button" class="btn btn-default" b-click="onCloseReviewModalClick()">انصرف</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!------------------------------------>
                        <!--لیست ریویو ها-->
                        <!------------------------------------>
                        <div class="review-wrapper" b-for="review in Reviews">
                            <div class="row">
                                <div class="col-1">
                                    <div class="icon-actions">
                                        <ul class="icons">
                                            <li>
                                                <i class="bi bi-bookmark"></i>
                                            </li>
                                            <li class="with-label">
                                                <i class="bi bi-heart" b-click="onSubmitLikeClick()"></i> 40
                                            </li>
                                            <li class="with-label">
                                                <i class="bi bi-chat-square"></i> 18
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="col-11 review-info">
                                    <div class="sender-info">
                                        <div class="image-wrapper">
                                            <img bind-image="review.ProfilePhoto" no-image="/portals/0/images/profile.png" />
                                        </div>
                                        <div class="more-info">
                                            <div class="text">
                                                <label class="name" bind-text="review.DisplayName"></label>
                                                <span b-show="review.IsAdmin == 'true'" class="admin-review"> ثبت شده توسط  مدیر سامانه</span>
                                                <span class="role" bind-text="review.Categories"></span>
                                            </div>
                                            <div class="score">
                                                <div class="score-result">
                                                    <span bind-text="review.Rate"></span>
                                                    <i class="bi bi-star-fill"></i>
                                                </div>
                                                <div class="score-action">
                                                    <i class="bi bi-star"></i>
                                                    <span>درج امتیاز</span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="article">
                                        <p class="description" bind-text="review.Review"></p>
                                        <div class="date-more">
                                            <div class="date">
                                                <span class="relative-date" bind-date="review.Date" format="HH:mm"></span> &nbsp;/&nbsp;
                                                <span class="standard-date" bind-date="review.Date" format=" dddd dd MMM yyyy"></span>
                                            </div>
                                            <a href="#" class="more-button right">
                                                <i class="bi bi-plus"></i> ادامه
                                            </a>
                                        </div>
                                    </div>
                                    <div class="ignore-ssr review-footer text-center mt-10 mb-10">
                                        <a class="lnk-view-comments" data-bs-toggle="collapse" href="#collapseExample" role="button" aria-expanded="false" aria-controls="collapseExample">
                                            {{review.Comments.length}} نظر
                                            |
                                            مشاهده نظرات
                                        </a>
                                    </div>
                                    <div class="collapse" id="collapseExample">
                                        <div class="card card-body alert alert-warning text-center" b-show="!review.Comments.length">
                                            هیچ نظری برای این ریویو ثبت نشده است.
                                            <button type="button" class="mt-10 btn-azadi-1" b-click="onShowCommentModal(review.ReviewID)">
                                                <i class="bi bi-plus-square-fill"></i>
                                                <span>ثبت نظر جدید</span>
                                            </button>
                                        </div>
                                        <!------------------------------------>
                                        <!--لیست کامنت ها-->
                                        <!------------------------------------>
                                        <div class="comment-list" b-show="review.Comments.length">
                                            <div class="comment" b-for="item in review.Comments">
                                                <div class="profile-wrapper">
                                                    <img bind-image="item.ProfilePhoto" no-image="/portals/0/images/profile-image.png" />
                                                </div>
                                                <div class="info-wrapper">
                                                    <div class="header-wrapper">
                                                        <h4 class="user-name" bind-text="item.DisplayName"></h4>
                                                        <span class="comment-date" bind-date="item.Date" format="dddd dd MMM yyyy ، HH:mm"></span>
                                                    </div>
                                                    <p class="comment-text" bind-text="item.Comment"></p>
                                                    <div class="footer-wrapper">
                                                        <button type="button" class="reply-button">
                                                            <i class="bi bi-reply"></i>
                                                            پاسخ به این نظر
                                                        </button>
                                                        <div class="like-wrapper">
                                                            <div class="like-dislike">
                                                                <span class="number">12</span>
                                                                <i class="bi bi-dash-circle"></i>
                                                            </div>
                                                            <div class="like-dislike">
                                                                <span class="number">3</span>
                                                                <i class="bi bi-plus-circle"></i>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!------------------------------------>
                        <!--مودال ثبت کامنت-->
                        <!------------------------------------>
                        <div class="modal fade" id="wnSubmitComment" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-hidden="true">
                            <div class="modal-dialog modal-dialog-centered">
                                <div class="modal-content">
                                    <div class="modal-header">
                                        <h5 class="modal-title">ثبت نظر</h5>
                                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                    </div>
                                    <div class="modal-body">
                                        <div class="mb-5">
                                            <label class="form-label">متن نظر</label>
                                            <textarea class="b-input form-control" ng-model="Form.Comment" rows="4" placeholder="لطفا نظر خود را به صورت فارسی وارد نمایید."></textarea>
                                            <span class="art-small-text text-danger d-block" b-show="Form.CommentRequiredError">وارد نمودن این فیلد الزامی است.</span>
                                        </div>
                                    </div>
                                    <div class="modal-footer">
                                        <button type="button" class="btn btn-primary ms-2" b-click="onSubmitCommentClick()">ثبت نظر</button>
                                        <button type="button" class="btn btn-default" b-click="onCloseCommentModalClick()">انصرف</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="tab-pane fade" id="jostar" role="tabpanel" aria-labelledby="jostar-tab">
                    </div>
                    <div class="tab-pane fade" id="news" role="tabpanel" aria-labelledby="news-tab">
                    </div>
                </div>
            </div>
        </div>
        <div class="col-5">
            <div class="art-widget">
                <div class="art-in-list">
                    <h4 class="header-title">اثر در فهرست ها</h4>
                    <div class="item-list">
                        <div class="item art-border">
                            <div class="image-wrapper">
                                <i class="bi bi-file-earmark-text"></i>
                            </div>
                            <div class="item-info">
                                <a class="item-link" href="#">
                                    خسروباباخانی در ویولون زن روی پل شعبده های ترک اعتیار را به چالش کشید
                                </a>
                                <div class="item-date">
                                    <span class="date-relative">2 ساعت پیش /</span>
                                    <span class="date-standard">دوشنبه 2 آبان 1401</span>
                                </div>
                            </div>
                        </div>
                        <div class="item art-border">
                            <div class="image-wrapper">
                                <i class="bi bi-file-earmark-text"></i>
                            </div>
                            <div class="item-info">
                                <a class="item-link" href="#">
                                    تمجید استاد شفیعی کدکندی از اثر ویولون زن روی پل
                                </a>
                                <div class="item-date">
                                    <span class="date-relative">3 ساعت پیش /</span>
                                    <span class="date-standard">دوشنبه 2 آبان 1401</span>
                                </div>
                            </div>
                        </div>
                        <div class="item art-border">
                            <div class="image-wrapper">
                                <i class="bi bi-file-earmark-text"></i>
                            </div>
                            <div class="item-info">
                                <a class="item-link" href="#">
                                    ویولوزن روی پل نازمدم جشنواره بهترین اثر سال شد
                                </a>
                                <div class="item-date">
                                    <span class="date-relative">دیروز /</span>
                                    <span class="date-standard">یکشنبه 1 آبان 1401</span>
                                </div>
                            </div>
                        </div>
                        <div class="item art-border">
                            <div class="image-wrapper">
                                <i class="bi bi-file-earmark-text"></i>
                            </div>
                            <div class="item-info">
                                <a class="item-link" href="#">
                                    خسروباباخانی در ویولون زن روی پل شعبده های ترک اعتیار را به چالش کشید
                                </a>
                                <div class="item-date">
                                    <span class="date-relative">2 ساعت پیش /</span>
                                    <span class="date-standard">دوشنبه 2 آبان 1401</span>
                                </div>
                            </div>
                        </div>
                        <div class="item art-border">
                            <div class="image-wrapper">
                                <i class="bi bi-file-earmark-text"></i>
                            </div>
                            <div class="item-info">
                                <a class="item-link" href="#">
                                    در بازی جرئت یا حفیف شما هر دو را انتخاب کردید یا هیچ؟
                                </a>
                                <div class="item-date">
                                    <span class="date-relative">2 ساعت پیش /</span>
                                    <span class="date-standard">یکشنبه 1 آبان 1401</span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="footer-links">
                        <a href="#" class="more-link fs-6">
                            مطالب بیشتر
                            <i class="bi bi-chevron-down me-2"></i>
                        </a>
                    </div>
                </div>
            </div>
            <div class="art-widget">
                <div class="another-arts">
                    <h4 class="header-title">سایر آثار این مولف</h4>
                    <div class="item-list" ng-repeat="item in AnotherArts">
                        <div class="item">
                            <div class="image-wrapper">
                                <img bind-image="item.Photo" />
                            </div>
                            <div class="item-info">
                                <h3 class="item-title" bind-text="item.Title"></h3>
                                <span class="author" bind-text="item.ArtistName"></span>
                                <p class="item-desc" bind-text="item.Summary">

                                </p>
                                <a href="/art?a={{item.ArtID}}" class="more-link">اطلاعات بیثشتر</a>
                            </div>
                        </div>

                    </div>
                    <div class="footer-links">
                        <a href="#" class="more-link fs-6">
                            مطالب بیشتر
                            <i class="bi bi-chevron-down me-2"></i>
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<link rel="stylesheet" href="/DesktopModules/BusinessEngine/client-resources/components/bootstrap-star-rating/css/star-rating.min.css" />
<link rel="stylesheet" href="/DesktopModules/BusinessEngine/client-resources/components/bootstrap-star-rating/themes/krajee-svg/theme.min.css" />
<script type="text/javascript" src="/DesktopModules/BusinessEngine/client-resources/components/bootstrap-star-rating/js/star-rating.min.js">
</script>
<script type="text/javascript" src="/DesktopModules/BusinessEngine/client-resources/components/bootstrap-star-rating/themes/krajee-svg/theme.min.js">
</script>
<script type="text/javascript" src="/DesktopModules/BusinessEngine/client-resources/components/bootstrap-star-rating/js/locales/fa.js">
</script>