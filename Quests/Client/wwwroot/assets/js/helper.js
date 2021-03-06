﻿"use strict";
function BlazorInit(helper) {

    ////////////////////////////////////////////////////
    // Layout Base Partials(mandatory for core layout)//
    ////////////////////////////////////////////////////

    // Init Desktop & Mobile Headers
    KTLayoutHeader.init('kt_header', 'kt_header_mobile');

    // Init Header Menu
    KTLayoutHeaderMenu.init('kt_header_menu', 'kt_header_menu_wrapper');

    // Init Header Topbar For Mobile Mode
    KTLayoutHeaderTopbar.init('kt_header_mobile_topbar_toggle');

    // Init Brand Panel For Logo
    KTLayoutBrand.init('kt_brand');

    // Init Aside
    KTLayoutAside.init('kt_aside');

    // Init Aside Menu Toggle
    KTLayoutAsideToggle.init('kt_aside_toggle');

    // Init Aside Menu
    KTLayoutAsideMenu.init('kt_aside_menu');

    // Init Subheader
    KTLayoutSubheader.init('kt_subheader');

    // Init Content
    KTLayoutContent.init('kt_content');

    // Init Footer
    KTLayoutFooter.init('kt_footer');


    //////////////////////////////////////////////
    // Layout Extended Partials(optional to use)//
    //////////////////////////////////////////////

    // Init Scrolltop
    KTLayoutScrolltop.init('kt_scrolltop');

    // Init Sticky Card
    KTLayoutStickyCard.init('kt_page_sticky_card');

    // Init Stretched Card
    KTLayoutStretchedCard.init('kt_page_stretched_card');

    // Init Code Highlighter & Preview Blocks(used to demonstrate the theme features)
	//KTLayoutExamples.init();

    // Init Demo Selection Panel
	KTLayoutDemoPanel.init('kt_demo_panel');

    // Init Chat App(quick modal chat)
    KTLayoutChat.init();

    // Init Quick Actions Offcanvas Panel
    KTLayoutQuickActions.init('kt_quick_actions');

    // Init Quick Notifications Offcanvas Panel
    KTLayoutQuickNotifications.init('kt_quick_notifications');

    // Init Quick Offcanvas Panel
    KTLayoutQuickPanel.init('kt_quick_panel');

    // Init Quick User Panel
    KTLayoutQuickUser.init('kt_quick_user');
    
    // Init Quick Search Panel
    KTLayoutQuickSearch.init('kt_quick_search');

    // Init Quick Cart Panel
    KTLayoutQuickCartPanel.init('kt_quick_cart');

    // Init Search For Quick Search Dropdown
    KTLayoutSearch().init('kt_quick_search_dropdown');

    // Init Search For Quick Search Offcanvas Panel
    KTLayoutSearchOffcanvas().init('kt_quick_search_offcanvas');

    helper.invokeMethodAsync("InvokeMethod",true);

    function eventFire(el, etype){
        if (el.fireEvent) {
            el.fireEvent('on' + etype);
        } else {
            var evObj = document.createEvent('Events');
            evObj.initEvent(etype, true, false);
            el.dispatchEvent(evObj);
        }
    }

    $('#kt_quick_user_toggle_mobile').click(function() {
        eventFire(document.getElementById('kt_quick_user_toggle'), 'tap');
       // $('#kt_quick_user_toggle').slideToggle();
    });
}

function DisableLoader() {
    $("#loader").remove();
}

function RbkMoneyCheckout(invoiceId,invoiceAccessToken,helper) {

    const checkout = RbkmoneyCheckout.configure({

        invoiceID: invoiceId,
        invoiceAccessToken: invoiceAccessToken,
        name: 'Хочуквест',
        description: 'Пополнение счета',
        opened: function () {
            helper.invokeMethodAsync("InvokeMethodOpen");
        },
        closed: function () {
            
        },
        finished: function () {
            helper.invokeMethodAsync("InvokeMethod");
        }

    });

    checkout.open();

    window.addEventListener('popstate', function() {
        checkout.close();
    });
}

function CloudPayment(invoiceId, sum, userId,helper) {
    var widget = new cp.CloudPayments();
    widget.pay('auth', // или 'charge'
        { //options
            publicId: 'pk_d515c410b5609d15c0c94471d9919', //id из личного кабинета
            description: 'Оплата услуг в hochuquest.com', //назначение
            amount: sum, //сумма
            currency: 'RUB', //валюта
            skin: "modern", //дизайн виджета (необязательно)
            accountId: userId, //идентификатор плательщика (необязательно)
            invoiceId: invoiceId, //номер заказа  (необязательно)
          },
        {
            onSuccess: function (options) { // success
                //действие при успешной оплате
                helper.invokeMethodAsync("InvokeMethod");
            },
            onFail: function (reason, options) { // fail
                //действие при неуспешной оплате
            },
            onComplete: function (paymentResult, options) { //Вызывается как только виджет получает от api.cloudpayments ответ с результатом транзакции.
                //например вызов вашей аналитики Facebook Pixel
            }
        }
    )
}
