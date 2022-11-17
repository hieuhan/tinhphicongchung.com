var app = {
    init: function () {
        this.events();
        this.initselect2();
        this.initToastr();
    },
    ajaxEvents :
    {
        OnBegin: function () {
            //showLoading();
            app.resetResult();
        },
        OnComplete: function (element) {
            //hideLoading();
        },
        OnSuccess: function (response, status, xhr) {
            if (response != null) {
                $('span[id*="ValidationMessageFor"].text-danger').removeClass('text-danger').text('Bạn vui lòng chọn từ trên xuống dưới');
                if (response.Message != null) {
                    if (!response.Completed && response.FieldValidationError != null) {
                        $('.row-ketqua').hide();
                        $(`#${response.FieldValidationError}`).first().focus();
                        $(`#ValidationMessageFor${response.FieldValidationError}`).addClass('text-danger').text(response.Message);   
                        return;
                    }
                    var username = '', msg = response.Message;
                    if (response.Message.indexOf('The provided anti-forgery token was meant for user') > -1) {
                        response.Message = response.Message.replace(/"/g, "'");
                        var str = response.Message.match(/'(.*?)'/g);
                        if (str != null) username = str[1];

                        msg = username.length > 0
                            ? 'Tài khoản: <b>' +
                            username +
                            '</b> hiện đã đăng nhập. Quý khách vui lòng refresh lại trang web để tiếp tục sử dụng.'
                            : response.Message;
                        msg += '<p>Để được hỗ trợ về đăng nhập tài khoản. Quý khách vui lòng liên hệ với LuatVietnam:</p>';
                        msg += '<p>Hotline: <a href="tel:0938361919" title="Hotline: 0938361919" style="color: #ed1d35">0938361919</a></p>';
                        msg += '<p>Email: <a href="mailto:cskh@luatvietnam.vn" title="Email: cskh@luatvietnam.vn">cskh@luatvietnam.vn</a></p>';
                    }

                    toastr.error(msg, 'Thông báo');
                } else if (response.Completed) {
                    if (response.Data == null || response.Data != 'NoAction')
                        $('.row-ketqua').show();
                    else $('.row-ketqua').hide();
                    if (response.ReturnUrl != null) {
                        window.location.href = response.ReturnUrl;
                        return;
                    }

                    if (response.Cb != null && response.Cb.length > 0) {
                        eval(atob(response.Cb));
                    }
                } else {
                    if (response.ReturnUrl != null) {
                        window.location.href = response.ReturnUrl;
                        return;
                    }
                }
            }
            //hideLoading();
        },
        OnFailed: function () {
            //hideLoading();
            app.resetResult();
            toastr.error('Quý khách vui lòng thử lại sau.', 'Thông báo');
        }
    },
    events: function () {
        $(document).on('change', '.select-onchange', function (e) {
            var form = $(this).closest('form');
            if (form) form.submit();
        })
        $(document).on('change', '.load-districts', function () {
            var self = $(this), provinceId = self.val() || 0,
                select_districts = $('select[name="DistrictId"]'),
                select_wards = $('select[name="WardId"]'),
                select_streets = $('select[name="StreetId"]');

            select_districts.empty().append($('<option/>', {
                'value': '0',
                'text': 'Chọn Quận / Huyện'
            }));

            select_wards.empty().append($('<option/>', {
                'value': '0',
                'text': 'Chọn Phường / Xã'
            }))

            select_streets.empty().append($('<option/>', {
                'value': '0',
                'text': 'Chọn Đường / Phố'
            }))

            app.fetchData(
                {
                    url: '/api/district/get.html',
                    dataType: 'json',
                    type: 'get',
                    data:
                    {
                        provinceId: provinceId
                    },
                    beforeSend: function () {
                        select_districts.find('option')
                            .removeAttr('selected')
                            .filter('[value="0"]')
                            .text('Đang tải dữ liệu...')
                            .attr('selected', true)
                    },
                    always: function () {
                        select_districts.find('option')
                            .removeAttr('selected')
                            .filter('[value="0"]')
                            .text('Chọn Quận / Huyện')
                            .attr('selected', true)
                    }
                })
                .then((response) => {
                    if (response != null && response.length > 0) {
                        $.each(response, function (i, obj) {
                            select_districts.append($('<option></option>').val(obj.Id)
                                .html(obj.Name));
                        });
                    }
                })
                .catch((error) => {
                    console.error(error)
                })
        })
        $(document).on('change', '.load-wards', function () {
            var self = $(this), districtId = self.val() || 0,
                select_wards = $('select[name="WardId"]'),
                select_streets = $('select[name="StreetId"]'),
                select_location_types = $('select[name="LocationId"]'),
                form = self.closest('form');

            select_wards.empty().append($('<option/>', {
                'value': '0',
                'text': 'Chọn Phường / Xã'
            }))

            select_streets.empty().append($('<option/>', {
                'value': '0',
                'text': 'Chọn Đường / Phố'
            }))

            select_location_types.empty().append($('<option/>', {
                'value': '0',
                'text': '--Chọn Vị trí--'
            }))

            app.fetchData(
                {
                    url: '/api/ward/get.html',
                    dataType: 'json',
                    type: 'get',
                    data:
                    {
                        districtId: districtId
                    },
                    beforeSend: function () {
                        select_wards.find('option')
                            .removeAttr('selected')
                            .filter('[value="0"]')
                            .text('Đang tải dữ liệu...')
                            .attr('selected', true);
                    },
                    always: function () {
                        select_wards.find('option')
                            .removeAttr('selected')
                            .filter('[value="0"]')
                            .text('Chọn Phường / Xã')
                            .attr('selected', true);

                        if (jQuery.fn.select2) {
                            $('select[name="WardId"].select2').select2();
                        }
                    }
                })
                .then((response) => {
                    if (response != null && response.length > 0) {
                        $.each(response, function (i, obj) {
                            select_wards.append($('<option></option>').val(obj.Id)
                                .html(obj.Name));
                        });

                        if (jQuery.fn.select2) {
                            $('select[name="WardId"].select2').select2();
                        }

                        if (form) form.submit();
                    }
                })
                .catch((error) => {
                    console.error(error)
                })
        })
        $(document).on('change', '.load-streets', function () {
            var self = $(this), wardId = self.val() || 0,
                select_streets = $('select[name="StreetId"]'),
                select_location_types = $('select[name="LocationId"]'),
                form = self.closest('form');

            select_streets.empty().append($('<option/>', {
                'value': '0',
                'text': 'Chọn Đường / Phố'
            }))

            select_location_types.empty().append($('<option/>', {
                'value': '0',
                'text': '--Chọn Vị trí--'
            }))

            app.fetchData(
                {
                    url: '/api/street/get.html',
                    dataType: 'json',
                    type: 'get',
                    data:
                    {
                        wardId: wardId
                    },
                    beforeSend: function () {
                        select_streets.find('option')
                            .removeAttr('selected')
                            .filter('[value="0"]')
                            .text('Đang tải dữ liệu...')
                            .attr('selected', true)
                    },
                    always: function () {
                        select_streets.find('option')
                            .removeAttr('selected')
                            .filter('[value="0"]')
                            .text('Chọn Đường / Phố')
                            .attr('selected', true);

                        if (jQuery.fn.select2) {
                            $('select[name="StreetId"].select2').select2();
                        }
                    }
                })
                .then((response) => {
                    if (response != null && response.length > 0) {
                        $.each(response, function (i, obj) {
                            select_streets.append($('<option></option>').val(obj.Id)
                                .html(obj.Name));
                        });

                        if (jQuery.fn.select2) {
                            $('select[name="StreetId"].select2').select2();
                        }

                        if (form) form.submit();
                    }
                })
                .catch((error) => {
                    console.error(error)
                })
        })
        $(document).on('change', 'select[name="LandTypeId"].load-locations', function () {
            var self = $(this), landTypeId = self.val() || 0,
                streetId = $('select[name="StreetId"] option:selected').val(),
                select_location_types = $('select[name="LocationId"]'),
                form = self.closest('form');

            select_location_types.empty().append($('<option/>', {
                'value': '0',
                'text': '--Chọn Vị trí--'
            }))

            if (landTypeId > 0 && streetId > 0) {
                app.fetchData(
                    {
                        url: '/api/location/get.html',
                        dataType: 'json',
                        type: 'get',
                        data:
                        {
                            landTypeId: landTypeId,
                            streetId: streetId
                        },
                        beforeSend: function () {
                            select_location_types.find('option')
                                .removeAttr('selected')
                                .filter('[value="0"]')
                                .text('Đang tải dữ liệu...')
                                .attr('selected', true)
                        },
                        always: function () {
                            select_location_types.find('option')
                                .removeAttr('selected')
                                .filter('[value="0"]')
                                .text('--Chọn Vị trí--')
                                .attr('selected', true);

                            if (jQuery.fn.select2) {
                                $('select[name="LocationId"].select2').select2();
                            }
                        }
                    })
                    .then((response) => {
                        if (response != null && response.length > 0) {
                            $.each(response, function (i, obj) {
                                select_location_types.append($('<option></option>').val(obj.Id)
                                    .html(obj.Name));
                            });
                        }
                    })
                    .catch((error) => {
                        console.error(error)
                    })
                
            } else {
                if (jQuery.fn.select2) {
                    $('select[name="LocationId"].select2').select2();
                }
            }

            if (form) form.submit();
        })
        $(document).on('change', 'select[name="StreetId"].load-locations', function () {
            var self = $(this), streetId = self.val() || 0,
                landTypeId = $('select[name="LandTypeId"] option:selected').val(),
                select_location_types = $('select[name="LocationId"]'),
                form = self.closest('form');

            select_location_types.empty().append($('<option/>', {
                'value': '0',
                'text': '--Chọn Vị trí--'
            }))

            if (landTypeId > 0 && streetId > 0) {
                app.fetchData(
                    {
                        url: '/api/location/get.html',
                        dataType: 'json',
                        type: 'get',
                        data:
                        {
                            landTypeId: landTypeId,
                            streetId: streetId
                        },
                        beforeSend: function () {
                            select_location_types.find('option')
                                .removeAttr('selected')
                                .filter('[value="0"]')
                                .text('Đang tải dữ liệu...')
                                .attr('selected', true)
                        },
                        always: function () {
                            select_location_types.find('option')
                                .removeAttr('selected')
                                .filter('[value="0"]')
                                .text('--Chọn Vị trí--')
                                .attr('selected', true);

                            if (jQuery.fn.select2) {
                                $('select[name="LocationId"].select2').select2();
                            }
                        }
                    })
                    .then((response) => {
                        if (response != null && response.length > 0) {
                            $.each(response, function (i, obj) {
                                select_location_types.append($('<option></option>').val(obj.Id)
                                    .html(obj.Name));
                            });
                        }
                    })
                    .catch((error) => {
                        console.error(error)
                    })

            } else {
                if (jQuery.fn.select2) {
                    $('select[name="LocationId"].select2').select2();
                }
            }

            if (form) form.submit();
        })
        $(document).on('change',
            'select[name="Year"], select[name="Month"]',
            function () {
                var y = $('select[name="Year"] option:selected').val();
                var m = $('select[name="Month"] option:selected').val();
                var d = $('select[name="Day"] option:selected').val();
                app.getBirthDay(d, m, y);
            });
        $(document).on('click',
            '#select-all',
            function () {
                if (this.checked) {
                    $('.checkbox-select').each(function () {
                        $(this).prop('checked', true);
                        $(this).closest('td').find('input[type=hidden]').remove();
                    });
                } else {
                    $('.checkbox-select').each(function () {
                        $(this).prop('checked', false);
                        $(this).closest('td')
                            .append('<input type="hidden" value="' + $(this).val() + '" name="IdRemove">');
                    });
                }
            });
        $(document).on('change',
            '.checkbox-select',
            function () {
                if (this.checked) {
                    $(this).closest('td').find('input[type=hidden]').remove();
                } else {
                    var input = $(this).closest('td').find('input[type=hidden]');
                    if (input.length) {
                        input.val($(this).val());
                    } else {
                        $(this).closest('td')
                            .append('<input type="hidden" value="' + $(this).val() + '" name="IdRemove">');
                    }
                }
            });
        $(document).on('click',
            '.submit-form',
            function () {
                var table = $('.table-responsive').find('table').first();
                if (table.length > 0)
                    table.closest('form').submit();
            });
        $(document).on('click',
            '.btn-submit',
            function () {
                 $(this).closest('form').submit();
            });
        $(document).on('keypress keyup blur', '.currency', function (event) {
            if ((event.which < 48 || event.which > 57)) {
                event.preventDefault();
            }
            var value = $(this).val().replace(/^(0*)/, '');
            //form = $(this).closest('form');
            // reset errors with unobtrusive
            //form.trigger('reset.unobtrusiveValidation');
            $(this).val(app.currencyFormat(value));
        })
        $(document).on('keypress keyup blur', '.number', function (event) {
            if ((event.which < 48 || event.which > 57)) {
                event.preventDefault();
            }
            var value = $(this).val().replace(/^(0*)/, '');
            $(this).val(value);
        })
        $(document).on('keypress keyup keydown blur', '.double', function (event) {
            if (event.shiftKey == true) {
                event.preventDefault();
            }

            if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105) || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 46 || event.keyCode == 190) {

            } else {
                event.preventDefault();
            }

            if ($(this).val().indexOf('.') !== -1 && event.keyCode == 190)
                event.preventDefault();

            var value = $(this).val().replace(/^(0*)/, '');

            $(this).val(value);
        })
        var timeOut;
        $(document).on('keyup', '.currency, .number, .double', function (event) {
            clearTimeout(timeOut);
            var self = $(this), form = self.closest('form');
            timeOut = setTimeout(function () {
                form.submit();
            }, 600);
        })
        $(document).on('click', '.go-back', function (event) {
            window.history.go(-1);
        })
        $(document).on('click', '.btn-ketqua', function () {
            app.resetResult();
        })
    },
    resetResult: function () {
        //$('.row-ketqua').addClass('hidden');
        $('.row-ketqua').find('.gia-tri-nha-theo-khung-gia-nha-nuoc').first().html('');
        $('.row-ketqua').find('.phi-cong-chung').first().html('');
    },
    daysList: function (month, year) {
        var day = new Date(year, month, 0);
        return day.getDate();
    },
    getBirthDay: function (d, m, y) {
        var date = new Date(), days = app.daysList(m == 0 ? date.getMonth() + 1 : m, y == 0 ? date.getFullYear() : y),
            selectDay = $('select[name="Day"]');
        selectDay.html('<option value="0"> Ngày </option>');
        for (var i = 1; i <= days; i++) {
            selectDay.append('<option value="' + i + '" ' + (i == d ? 'selected' : '') + '>' + i + '</option>');
        };
    },
    fetchData: function (options) {
        return new Promise((resolve, reject) => {
            $.ajax({
                cache: options.cache || true,
                url: options.url,
                type: options.type || 'GET',
                data: options.data || {},
                dataType: options.dataType || 'json',
                beforeSend: options.beforeSend || showLoading(),
                success: function (data) {
                    resolve(data);
                },
                error: function (error) {
                    reject(error);
                }
            }).always(options.always || hideLoading());
        });
    },
    currencyFormat: function (num) {
        var str = num.toString().replace(',', '').replace('.',''), parts = false, output = [], i = 1, formatted = null;
        if (str.indexOf(".") > 0) {
            parts = str.split(".");
            str = parts[0];
        }
        str = str.split("").reverse();
        for (var j = 0, len = str.length; j < len; j++) {
            if (str[j] != ",") {
                output.push(str[j]);
                if (i % 3 == 0 && j < (len - 1)) {
                    output.push(",");
                }
                i++;
            }
        }
        formatted = output.reverse().join("");
        return (formatted + ((parts) ? "." + parts[1].substr(0, 2) : ""));
    },
    addParam: function (currentUrl, key, val) {
        var url = new URL(currentUrl);
        url.searchParams.set(key, val);
        return url.href;
    },
    setQueryParameter: function (uri, key, value) {
        var re = new RegExp("([?&])(" + key + "=)[^&#]*", "g");
        if (uri.match(re))
            return uri.replace(re, '$1$2' + value);

        // need to add parameter to URI
        var paramString = (uri.indexOf('?') < 0 ? "?" : "&") + key + "=" + value;
        var hashIndex = uri.indexOf('#');
        if (hashIndex < 0)
            return uri + paramString;
        else
            return uri.substring(0, hashIndex) + paramString + uri.substring(hashIndex);
    },
    pushState: function (object) {
        if (window.history && window.history.pushState) {
            var url = new URL(window.location.href);
            for (var key in object) {
                url.searchParams.set(key, object[key]);
            }
            window.history.pushState(null, '', url);
        }
    },
    showLoading: function () {

    },
    hideLoading: function () {

    },
    initselect2: function () {
        if (jQuery.fn.select2) {
            $('select.select2').select2();
        }
    },
    initToastr: function () {
        if (typeof window['toastr'] !== 'undefined') {
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": true,
                "progressBar": true,
                "positionClass": "toast-bottom-right",
                "preventDuplicates": false,
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "10000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
        }
    }
}
$(function () {
    app.init();
});