"use strict";
// Class definition
var KTDatatable = function () {
    var datatable = null;
    var initTable = function (helper) {

        $(document).ready(function () {
            //datatable = $(helper).DataTable();
            //console.log(datatable);
            if (datatable != null) {
                datatable.destroy();
            }
            datatable = $(helper).KTDatatable({
                data: {
                    saveState: { cookie: false }
                },
                //search: {
                //    input: $('#kt_datatable_search_query'),
                //    key: 'generalSearch'
                //},
                columns: [
                    {
                        field: 'Img',
                        title: 'Аватар',
                        type: 'image',
                        width: 100
                    },
                    {
                        field: 'Name',
                        title: 'Имя',
                        type: 'text',
                        width: 100

                    },
                    {
                        field: 'RoleName',
                        title: 'Роль',
                        type: 'text',
                        width: 100

                    },
                    {
                        field: 'Points',
                        title: 'Очки',
                        type: 'text'
                    },
                    {
                        field: 'Phone',
                        title: 'Телефон',
                        type: 'text'
                    },
                    {
                        field: 'UserName',
                        title: 'Логин',
                        type: 'text'
                    },
                    {
                        field: 'LatestSign',
                        title: 'Последний вход',
                        type: 'text'
                    },
                    {
                        field: 'ActiveQuestId',
                        title: 'ActiveQuestId',
                        type: 'text'
                    },
                    {
                        field: 'ActiveStateStatus',
                        title: 'ActiveStateStatus',
                        type: 'text'
                    },
                    {
                        field: 'Action',
                        type: 'action',
                        autoHide: false,
                        width: 150

                    },
                ],
            });
            //datatable.destroy();
        });
    }

    var destroyTable = function () {
        $(document).ready(function () {
            datatable.destroy();
        });
    }

    return {
        init: function(helper) {
            initTable(helper);
        },
        table: function() {
            return datatable;
        },
        destroy: function () {
            destroyTable();
        }
    }
}();



