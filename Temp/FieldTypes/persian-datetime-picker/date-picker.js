var bField_PersianDateTimePicker = function ($scope, field) {
    this.init = function () {
        var $mbdDateTime;
        var watches = [];

        function init() {

        }

        if (field.Settings && !field.Settings.DateType) field.Settings.DateType = 'date';

        function _initPersianDatePicker() {
            var template = '<div id="plotId" class="datepicker-plot-area {{cssClass}}">    {{#navigator.enabled}}        <div data-navigator class="datepicker-navigator">            <div class="pwt-btn pwt-btn-next"><i class="fa fa-angle-left f-15"></i></div>            <div class="pwt-btn pwt-btn-switch">{{navigator.switch.text}}</div>            <div class="pwt-btn pwt-btn-prev"><i class="fa fa-angle-right f-15"/></div>        </div>    {{/navigator.enabled}}    <div class="datepicker-grid-view" >    {{#days.enabled}}        {{#days.viewMode}}        <div class="datepicker-day-view" >                <div class="month-grid-box">                <div class="header">                    <div class="title"></div>                    <div class="header-row">                        {{#weekdays.list}}                            <div class="header-row-cell">{{.}}</div>                        {{/weekdays.list}}                    </div>                </div>                    <table cellspacing="0" class="table-days">                    <tbody>                        {{#days.list}}                                                       <tr>                                {{#.}}                                    {{#enabled}}                                        <td data-date="{{dataDate}}" data-unix="{{dataUnix}}" >                                            <span  class="{{#otherMonth}}other-month{{/otherMonth}}">{{title}}</span>                                            {{#altCalendarShowHint}}                                            <i  class="alter-calendar-day">{{alterCalTitle}}</i>                                            {{/altCalendarShowHint}}                                        </td>                                    {{/enabled}}                                    {{^enabled}}                                        <td data-date="{{dataDate}}" data-unix="{{dataUnix}}" class="disabled">                                            <span class="{{#otherMonth}}other-month{{/otherMonth}}">{{title}}</span>                                            {{#altCalendarShowHint}}                                            <i  class="alter-calendar-day">{{alterCalTitle}}</i>                                            {{/altCalendarShowHint}}                                        </td>                                    {{/enabled}}                                                                    {{/.}}                            </tr>                        {{/days.list}}                    </tbody>                </table>            </div>        </div>        {{/days.viewMode}}    {{/days.enabled}}        {{#month.enabled}}        {{#month.viewMode}}            <div class="datepicker-month-view">                {{#month.list}}                    {{#enabled}}                                       <div data-month="{{dataMonth}}" class="month-item {{#selected}}selected{{/selected}}">{{title}}</small></div>                    {{/enabled}}                    {{^enabled}}                                       <div data-month="{{dataMonth}}" class="month-item month-item-disable {{#selected}}selected{{/selected}}">{{title}}</small></div>                    {{/enabled}}                {{/month.list}}            </div>        {{/month.viewMode}}    {{/month.enabled}}        {{#year.enabled }}        {{#year.viewMode }}            <div class="datepicker-year-view" >                {{#year.list}}                    {{#enabled}}                        <div data-year="{{dataYear}}" class="year-item {{#selected}}selected{{/selected}}">{{title}}</div>                    {{/enabled}}                    {{^enabled}}                        <div data-year="{{dataYear}}" class="year-item year-item-disable {{#selected}}selected{{/selected}}">{{title}}</div>                    {{/enabled}}                                    {{/year.list}}            </div>        {{/year.viewMode }}    {{/year.enabled }}        </div>    {{#time}}    {{#enabled}}    <div class="datepicker-time-view">        {{#hour.enabled}}            <div class="hour time-segment" data-time-key="hour">                <div class="up-btn" data-time-key="hour">\u25B2</div>                <input disabled value="{{hour.title}}" type="text" placeholder="hour" class="hour-input">                <div class="down-btn" data-time-key="hour">\u25BC</div>                                </div>                   <div class="divider">                <span>:</span>            </div>        {{/hour.enabled}}        {{#minute.enabled}}            <div class="minute time-segment" data-time-key="minute" >                <div class="up-btn" data-time-key="minute">\u25B2</div>                <input disabled value="{{minute.title}}" type="text" placeholder="minute" class="minute-input">                <div class="down-btn" data-time-key="minute">\u25BC</div>            </div>                    <div class="divider second-divider">                <span>:</span>            </div>        {{/minute.enabled}}        {{#second.enabled}}            <div class="second time-segment" data-time-key="second"  >                <div class="up-btn" data-time-key="second" >\u25B2</div>                <input disabled value="{{second.title}}"  type="text" placeholder="second" class="second-input">                <div class="down-btn" data-time-key="second" >\u25BC</div>            </div>            <div class="divider meridian-divider"></div>            <div class="divider meridian-divider"></div>        {{/second.enabled}}        {{#meridian.enabled}}            <div class="meridian time-segment" data-time-key="meridian" >                <div class="up-btn" data-time-key="meridian">\u25B2</div>                <input disabled value="{{meridian.title}}" type="text" class="meridian-input">                <div class="down-btn" data-time-key="meridian">\u25BC</div>            </div>        {{/meridian.enabled}}    </div>    {{/enabled}}    {{/time}}        {{#toolbox}}    {{#enabled}}    <div class="toolbox">        {{#toolbox.submitButton.enabled}}            <div class="pwt-btn-submit">{{submitButtonText}}</div>        {{/toolbox.submitButton.enabled}}                {{#toolbox.todayButton.enabled}}            <div class="pwt-btn-today">{{todayButtonText}}</div>        {{/toolbox.todayButton.enabled}}                {{#toolbox.calendarSwitch.enabled}}            <div class="pwt-btn-calendar">{{calendarSwitchText}}</div>        {{/toolbox.calendarSwitch.enabled}}    </div>    {{/enabled}}    {{^enabled}}        {{#onlyTimePicker}}        <div class="toolbox">            <div class="pwt-btn-submit">{{submitButtonText}}</div>        </div>        {{/onlyTimePicker}}    {{/enabled}}    {{/toolbox}}</div>';

            $mbdDateTime = $("#mbdPersianDate" + field.FieldName).pDatepicker({
                autoClose: true,
                format: 'YYYY/MM/DD',
                template: template,
                initialValue: false,
                minDate: field.Settings.MinDate ? moment($scope.service.getScopePropertyValue($scope, field.Settings.MinDate)).unix() * 1000 : undefined,
                navigator: {
                    text:
                    {
                        btnPrevText: '<i class="fa fa-prev"/>'
                    },
                    scroll: {
                        enabled: false
                    }
                },
                onSelect: function (unix) {
                    _setFieldValue();
                }
            });
        }

        function _setFieldValue() {
            if ($('#mbdPersianDate' + field.FieldName).val()) {
                field.Value = moment(moment.unix($mbdDateTime.getState().selected.unixDate / 1000).format('MM/DD/YYYY') + ' ' + (field.ValueTime ? field.ValueTime : '00:00')).format('MM/DD/YYYY HH:mm');
                if (!$scope.$$phase) $scope.$apply();
            }
        }

        setTimeout(function () {
            if (field.Settings.MinDate) {
                matches = field.Settings.MinDate.match(/{(\w+([\.|\[\]\w]*))?}/gim);
                angular.forEach(matches, function (m) {
                    match = /{(\w+([\.|\[\]\w]*))?}/gim.exec(m);
                    if (match && match.length > 1) watches.push(match[1]);
                });
            }

            _initPersianDatePicker();

            if (field.Value) $mbdDateTime.setDate(new Date(field.Value).getTime());

            $('#mbdPersianDateTime' + field.FieldName).inputmask({ regex: '^([01][0-9]|2[0-3]):([0-5][0-9])$' });

            $("#mbdPersianDate" + field.FieldName).change(function () {
                var dt = moment($(this).val(), 'jYYYY/jM/jD');
                if (dt.isValid() && dt.year() > 1920)
                    field.Value = dt.format('MM/DD/YYYY');
                else {
                    delete field.Value;
                    $('#mbdPersianDate' + field.FieldName).val('');
                }

                if (!$scope.$$phase) $scope.$apply();
            });

            $scope.$watch('Field.' + field.FieldName + '.Value', function (newVal, oldVal) {
                if (newVal) {
                    $mbdDateTime.setDate(moment(newVal).unix() * 1000);

                    field.ValueTime = moment(newVal).format("HH:mm");

                    _setFieldValue();
                }
                else
                    $('#mbdPersianDate' + field.FieldName).val('');
            });

            if (!field.IsShow) {
                $scope.$watch('Field.' + field.FieldName + '.IsShow', function (newVal, oldVal) {
                    if (newVal != oldVal && newVal) {
                        setTimeout(function () {
                            _initPersianDatePicker();
                        });
                    }
                });
            }

            $scope.$watch('Field.' + field.FieldName + '.ValueTime', function (newVal, oldVal) {
                if (newVal != oldVal && /^([01][0-9]|2[0-3]):([0-5][0-9])$/.test(newVal)) {
                    _setFieldValue();
                }
            });

            if (watches.length) {
                $scope.$watch('[' + watches.join(',') + ']', function (newVal, oldVal) {
                    if (newVal != oldVal) {
                        _initPersianDatePicker();
                    }
                });
            }
        }, 1000);    }
}