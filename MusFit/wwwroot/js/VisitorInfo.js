        var createModal = document.getElementById("createModal");
        var createReserveModal = document.getElementById("createReserveModal");
        var editModal = document.getElementById("editModal");
        var deleteModal = document.getElementById("deleteModal");
        var errorMessage = document.getElementById('errorMessage');
        var validationMessage = document.getElementById('validationMessage');
        var valPhoneMessage = document.getElementById('valPhoneMessage');
        var valiMailMessage = document.getElementById('valMailMessage');
        var valVisitorMessage = document.getElementById('valVisitorMessage');
        const pageid = document.getElementById('pageid');
        var reg = /^(09)[0-9]{8}$/;

        //clear-createform
        function FormClear() {
            document.getElementById("createForm").reset();
            document.getElementById("createOrder").reset();
        }

        //close modals
        function closeModal() {
            createModal.style.display = "none";
            editModal.style.display = "none";
            deleteModal.style.display = "none";
            showNoDataModal.style.display = "none";
            createReserveModal.style.display = "none";
            errorMessage.style.display = "none";
        }

        document.getElementById('cancelCreate').addEventListener('click', closeModal);
        document.getElementById('cancelCreate').addEventListener('click', FormClear);
        document.getElementById('leaveCreate').addEventListener('click', closeModal);
        document.getElementById('leaveCreate').addEventListener('click', FormClear);
        document.getElementById('cancelReservation').addEventListener('click', closeModal);
        document.getElementById('cancelReservation').addEventListener('click', FormClear);
        document.getElementById('leaveReservation').addEventListener('click', closeModal);
        document.getElementById('leaveReservation').addEventListener('click', FormClear);

        pageid.addEventListener('click', switchPage);

        //add keypress events
        function addKeyPress(formID, btn) {
            document.getElementById(formID).addEventListener("keydown", function (event) {
                if (event.key === "Enter") {
                    event.preventDefault(); document.getElementById(btn).click();
                }
        });
}

        addKeyPress("createOrder", "btnCreateReserveConfirm");
        addKeyPress("createForm", "btnCreateConfirm");
        addKeyPress("queryinfo", "btnQuery");
        addKeyPress("startDate", "btnQuery");
        addKeyPress("endDate", "btnQuery");
        addKeyPress("editForm", "btnEditConfirm");
        addKeyPress("editOrder", "btnEditConfirm");


        //close validation remind modal
        function closeValidationModel() {
            validationMessage.style.display = "none";
            valPhoneMessage.style.display = "none";
            valMailMessage.style.display = "none";
            valVisitorMessage.style.display = "none";
        }

        //close success message modal
        function closeSuccessModel() {
            $('#messageModal').css("display", "none");
            location.reload();
}

        //email format validation
        function IsEmail(email) {
            var regex = /^([a-zA-Z0-9_\.\-\+])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            if (!regex.test(email)) { return false; } else { return true; }
        }

        //ajax type
        const AjaxType = {
            GET : 'get',
            DELETE : 'delete',
            POST : 'post',
            PUT: 'put',
        }

        //ajax function
        function myAJAX(type, url, successFunc, contentType, data) {
            $.ajax({
                type: type,
                url: url,
                contentType: contentType,
                data: data,
                success: successFunc
            })
        }

        //info pagination
        function pagination(jsonData, nowPage) {

            const dataTotal = jsonData.length;

            const perpage = 4;
            const pageTotal = Math.ceil(dataTotal / perpage);

            let currentPage = nowPage;
            if (currentPage > pageTotal) { currentPage = pageTotal; }

            const minData = (currentPage * perpage) - perpage + 1;
            const maxData = (currentPage * perpage);

            // dynamic data array according to current page
            const data = [];

            jsonData.forEach((item, index) => {
                const num = index + 1;
                if (num >= minData && num <= maxData) { data.push(item); }
            })
            const page = {
                pageTotal,
                currentPage,
                hasPage: currentPage > 1,  // has previous page
                hasNext: currentPage < pageTotal,  // has next page
            }
            displayData(data);
            pageBtn(page, nowPage);
        }

        function displayData(data){
            var tableRow;
            var gender;
            var date;
            var weekday;
            var dateInfo;
            data.forEach(function (o) {
                date = new Date(o.date).toLocaleDateString();
                weekday = o.weekday.substring(2);
                dateInfo = `${date}(${weekday})`;
                if (o.gender == false) { gender = '女'; } else { gender = '男'; }
                if (o.timeID == 63) { o.className = "尚未預約"; dateInfo = "尚未預約"; }
                tableRow += `<tr class="result">
                                        <td>${o.name}</td>
                                        <td class="gender">${gender}</td>
                                        <td class="phone">${o.phone}</td>
                                        <td class="mail">${o.mail}</td>
                                        <td>${o.className}</td>
                                        <td>${dateInfo}</td>
                                        <td>
                                        <button class="edit" onclick=openEditModel(${o.sID},${o.orderID})> 編輯 </button>
                                        <button class="delete" onclick = "opendeleteModel(${o.orderID},${o.classRecordID})" > 刪除 </button>
                                        </td>
                                    </tr>`; 
            })
            $('.result').remove();
            $('#myTable').append(tableRow);
        }

        function pageBtn(page, nowPage) {
                const totalPage = page.pageTotal;
                let pagination = document.querySelector('#pageid');
                var pageHtml = "", prevButton, nextButton;
                let pageIndex = parseInt(nowPage);

                if (page.hasPage) {
                    prevButton = `<li class='list-items' id='btnPrev'><a class="page-link" href="#" data-page="${Number(page.currentPage) - 1}">&laquo;</a></li>`;
                } else {
                    prevButton = `<li class='list-items' id='btnPrev'><a class="page-link" href="#"  data-page="${pageIndex}">&laquo;</a></li>`;
                }
                if (page.hasNext) {
                    nextButton = `<li class='list-items' id='btnNext'><a class="page-link" href="#" data-page="${Number(page.currentPage) + 1}">&raquo;</a></li>`;
                } else {
                    nextButton = `<li class='list-items' id='btnNext'><a class="page-link" href="#" data-page="${pageIndex}" >&raquo;</a></li>`;
                }

                let firstPage = `<li class='list-items' pagenumber=1><a class="page-link" href="#" data-page="${i}">1</a></li>`;
                let lastPage = `<li class='list-items' pagenumber=${totalPage}><a class="page-link" href="#" data-page="${totalPage}">${totalPage}</a></li>`;
                let leftOmitPage = `<li class='list-items' id='btnGoLeft'><a class="page-link" href="#" data-page="${i}">...</a></li>`;
                let rightOmitPage = `<li class='list-items' id='btnGoRight'><a class="page-link" href="#" data-page="${totalPage}">...</a></li>`;
                pageHtml = prevButton;

                if (totalPage <= 10) {
                    // totalPage is less than 10 pages
                    for (let i = 1; i <= totalPage; i++) {
                        pageHtml += `<li class='list-items' pagenumber=${i}><a class="page-link" href="#" data-page="${i}">${i}</a></li>`;
                    }
                }
                else if (pageIndex <= 8) {
                    // if currentPage is at the start
                    for (let i = 1; i <= 9; i++) {
                        pageHtml += `<li class='list-items' pagenumber=${i}><a class="page-link" href="#" data-page="${i}">${i}</a></li>`;
                    }
                    pageHtml += rightOmitPage;
                    pageHtml += lastPage;
                }
                else if (pageIndex > totalPage - 7) {
                    // if currentPage is at the end
                    pageHtml += firstPage;
                    pageHtml += leftOmitPage;
                    for (let i = totalPage - 8; i <= totalPage; i++) {
                        pageHtml += `<li class='list-items' pagenumber=${i}><a class="page-link" href="#" data-page="${i}">${i}</a></li>`;
                    }
                }
                else {
                    // if currentPage is in middle
                    pageHtml += firstPage;
                    pageHtml += leftOmitPage;
                    for (let i = pageIndex - 3; i <= pageIndex + 3; i++) {
                        pageHtml += `<li class='list-items' pagenumber=${i}><a class="page-link" href="#" data-page="${i}">${i}</a></li>`;
                    }
                    pageHtml += rightOmitPage;
                    pageHtml += lastPage;
                }
                pageHtml += nextButton;
                pagination.innerHTML = pageHtml;
                if (totalPage != 0) {
                    document.querySelector("li[pagenumber='" + pageIndex + "']").classList.add('active');
                }
        }

        function switchPage(e) {
            e.preventDefault();
            if (e.target.nodeName !== 'A') return;
            const nowpage = e.target.dataset.page; // nowpage = "data-page"
            //pagination(jsonData, nowpage);
            if(queryData.length != 0){
                pagination(queryData, nowpage);
            }else{
                pagination(jsonData, nowpage);
            }
        }

        //load all reservations     
        myAJAX(AjaxType.GET, "/api/classorders/guestorders/",getAllVisitorReservations)
        var jsonData = [];
        function getAllVisitorReservations(data){
            jsonData = data;
            pagination(jsonData, 1);
        }

        //query reservation
        var queryData = [];
        var flag;
        var queryInput;     
        var queryStartDate;
        var queryEndDate;
        $('#btnQuery').click(function () {
            flag = false;
            queryInput = $('#queryinfo').val().trim();
            queryStartDate = $('#startDate').val();
            queryEndDate = $('#endDate').val();
            if (queryInput == "" && queryStartDate == "" && queryEndDate == "") {
                $('#errorMessage').css("display", "block");
                $('#confirmError').focus();
            }
            else {
                $('.result').remove();
                if (queryInput != "") {
                    myAJAX(AjaxType.GET, "/api/classorders/guestorders/", findVisitorsRevservation);
                } else if (queryStartDate != "" && queryEndDate != "") {
                    myAJAX(AjaxType.GET, "/api/classorders/guestorders/datequery/" + queryStartDate + "/" + queryEndDate, findReservationsByQueryDate);
                } 
            } 
        })
        function findVisitorsRevservation(data){        
            queryData = [];
            data.forEach(function (x) {
                if (x.name == queryInput || x.phone == queryInput) {
                    queryData.push(x);
                    flag = true;
                }
            })
            pagination(queryData, 1);
            if (flag == false) {
                $('#showNoDataModal').css("display", "block");
                $('#confirmNoData').focus();
            }
        }
        function findReservationsByQueryDate(data) {
            queryData = [];
            if (data.length != 0) {
                data.forEach(function (x) {
                    queryData.push(x);
                    flag = true;
                })              
            }
            pagination(queryData, 1);
            if (flag == false) {
                $('#showNoDataModal').css("display", "block");
                $('#confirmNoData').focus();
            }
        }

        //query start date select end date together 
        function queryStartDateChange(x) {
            var date = x.value;
            $('#endDate').val(date);
            $('#endDate').addClass("has-value");
        }

        //show all reservations
        $('#btnShowAll').click(function () {
            location.reload();
        })

        //open create visitor form
        function openCreateModel() {
            createModal.style.display = "block";
        }

        //open create reservation form
        function openCreateReserveModel() {
            createReserveModal.style.display = "block";
            $('#new_className').children("option").filter("[value != none]").remove();
            $('#visitorName').children("option").filter("[value != none]").remove();
            $('#new_classDate').children("option").filter("[value != none]").remove();
            myAJAX(AjaxType.GET, "/api/classes/",getAllClassName);
        }
        function getAllClassName(data){
            data.forEach(function (c) {
                var option = `<option value="${c.cName}">${c.cName}</option>`;
                $('#new_className').append(option);
            })
        }

        //create visitor
        $('#btnCreateConfirm').click(function () {
            var name = $('#CreateName').val().trim();
            var phone = $('#CreatePhone').val().trim();
            var mail = $('#CreateMail').val().trim();
            $('#CreateName').val(name);
            $('#CreatePhone').val(phone);
            $('#CreateMail').val(mail);

            if (name == "" || phone == "" || mail == "" || $('input:radio[name=sGender]:checked').val() == 0) {
                validationMessage.style.display = "block"; $('#confirmValidation').focus(); }
            else if (!reg.test(phone)) { valPhoneMessage.style.display = "block"; $('#confirmValPhone').focus(); }
            else if (!IsEmail(mail)) { valMailMessage.style.display = "block"; $('#confirmValMail').focus(); }
            else {
                myAJAX(AjaxType.POST, "/api/students/", showMessage("已存檔"), "application/json", JSON.stringify(GetFormData($('#createForm'))));
            }
        })


        function showMessage(message){
            $('#createModal').css("display", "none");
            $('#createReserveModal').css("display", "none");
            $('#deleteModal').css("display", "none");
            $('#editModal').css("display", "none");  
            $('#message').text(message);
            $('#messageModal').css("display", "block");
            $('#confirmMessage').focus();
        }

        //create reservation
        $('#btnCreateReserveConfirm').click(function () {
            var new_className = $('#new_className').val();
            var new_classDate = $('#new_classDate').val();
            var classRecords = { sId: $('#new_sID').val(), classTimeId: $('#new_classTimeId').val() };

            if ($('#new_sID').val() == "") { valVisitorMessage.style.display = "block"; $('#confirmValVisitor').focus(); }
            else if (new_className == null || new_classDate == null) { validationMessage.style.display = "block"; $('#confirmValidation').focus(); }
            else {
                $('#new_orderStatus').val('體驗');

                $('#new_orderTime').val(new Date().toJSON().slice(0, 19));
                // create classrecords
                myAJAX(AjaxType.POST, "/api/classrecords/", null, "application/json", JSON.stringify(classRecords));
                // create reservations
                myAJAX(AjaxType.POST, "/api/classorders/", showMessage("已存檔"),"application/json", JSON.stringify(GetFormData($('#createOrder'))))
            }
        })

        //delete reservation
        $('#btnconfirmdelete').click(function () {
            var orderID = $('#delOrderID').val();
            var classRecordID = $('#delClassRecordID').val();
            myAJAX(AjaxType.DELETE, "/api/classrecords/" + classRecordID);
            myAJAX(AjaxType.DELETE, "/api/classorders/" + orderID, showMessage("已刪除"));
        })

        //open edit modal
        function openEditModel(sID, orderID) {

            $('#sId').val(sID);
            //get visitor personal info
            myAJAX(AjaxType.GET, "/api/students/" + sID, getVisitorPersonalInfo);

            //get visitor reservation info
            myAJAX(AjaxType.GET, "/api/classorders/" + orderID, getVisitorReservationsInfo);
            editModal.style.display = "block";
        }
     
        var classID;
        var classTimeID;
        var sID = $('#sId').val();
        var crAttendance;
        var crContent;
        var crID;
        function getVisitorPersonalInfo(e){
             $('#sId').val(e.sId);
             $('#name').val(e.sName);
             $('#phone').val(e.sPhone);
             $('#mail').val(e.sMail);
            $('#sNumber').val(e.sNumber);
            if (e.sContactor == null) { $('#sContactor').val("null") }
            else { $('#sContactor').val(e.sContactor); }
            if (e.sContactPhone == null) { $('#sContactPhone').val("null") }
            else { $('#sContactPhone').val(e.sContactPhone); }
            if (e.sPhoto == null) { $('#sPhoto').val("null") }
            else { $('#sPhoto').val(e.sPhoto); }
            if (e.sAddress == null) { $('#sAddress').val("null") }
            else { $('#sAddress').val(e.sAddress); }
            if (e.sAccount == null) { $('#sAccount').val("null") }
            else { $('#sAccount').val(e.sAccount); }
            if (e.sPassword == null) { $('#sPassword').val("null") }
            else { $('#sPassword').val(e.sPassword); }
            if (e.sToken == null) { $('#sToken').val("null") }
            else { $('#sToken').val(e.sToken); }
             if (e.sBirth == null) { $('#sBirth').val("null"); }
             else { $('#sBirth').val(e.sBirth); }
             if (e.sJoinDate == null) { $('#sJoinDate').val("null"); }
             else { $('#sJoinDate').val(e.sJoinDate); }
             $('#sIsStudentOrNot').val(e.sIsStudentOrNot);
             if (e.sGender == false) { $('input[name=sGender]')[1].checked = true; }
             else { $('input[name=sGender]')[0].checked = true; }
        }
        function getVisitorReservationsInfo(x){
            $('#orderId').val(x[0].orderID);
            $('#eId').val(x[0].eID);
            $('#orderStatus').val(x[0].orderStatus);
            $('#sID').val(x[0].sID);
            classID = x[0].cID;
            classTimeID = x[0].timeID;
            // get reserve classname
            myAJAX(AjaxType.GET, "/api/classes/", getVisitorReserveClassName);
            // get reserve classdate
            myAJAX(AjaxType.GET, "/api/classTimes/term/", getVisitorReserveClassDate);
            // get reservation classrecords
            myAJAX(AjaxType.GET, "/api/classrecords/sId/classTimeID/" + x[0].sID + "/" + x[0].timeID, getVisitorReserveClassRecords);
        }
        function getVisitorReserveClassName(data){
            $('#className').empty();
            data.forEach(function (c) {
                var option;
                if (c.cId == classID) {
                    // classID == 11 means no appointment yet
                    if (classID == 11) { option = `<option value="0" selected hidden disabled>----------尚未預約----------</option>`; }
                    else {   option = `<option value="${c.cName}" selected>${c.cName}</option>`;}      
                }
                else {
                    // classID == 11 means no appointment yet
                    if (c.cId != 11) {option = `<option value="${c.cName}">${c.cName}</option>`; }
                    
                }
                $('#className').append(option);
            })
        }
        function getVisitorReserveClassDate(data){
             $('#classDate').empty();
             data.forEach(function (t) {
                 var option;
                 var date = new Date(t.ctDate).toLocaleDateString();
                 var weeday = t.weekday.substring(2);
                 if (t.cId == classID) {
                     if (t.classTimeId == classTimeID) {
                         // classID == 63 means no appointment yet
                         if (classTimeID == 63) { option = `<option value="0" selected hidden disabled>----------尚未預約----------</option>`; }
                         else { option = `<option value="${t.ctDate}" selected>${date}(${weeday}) ${t.startTime}~${t.endTime}</option>`;}
                     }
                    else { option = `<option value="${t.ctDate}">${date}(${weeday}) ${t.startTime}~${t.endTime}</option>`; }
                  }
                 $('#classDate').append(option);
             })
}
        function getVisitorReserveClassRecords(data) {
            crID = data[0].crId;
            crAttendance = data[0].crAttendance;
            crContent = data[0].crContent;
        }

        //edit reservation information
        $('#btnEditConfirm').click(function () {       
            var name = $('#name').val().trim();
            var phone = $('#phone').val().trim();
            var mail = $('#mail').val().trim();
            $('#name').val(name);
            $('#phone').val(phone);
            $('#mail').val(mail);
            var className = $('#className').val();
            var classDate = $('#classDate').val();
            var classRecords = { crId: crID, sId: $('#sId').val(), classTimeId: $('#classTimeId').val(), crAttendance: crAttendance, crContent: crContent };

            if (name == "" || phone == "" || mail == "" || $('input:radio[name=sGender]:checked').val() == 0
                || className == null || classDate == null) {
                validationMessage.style.display = "block";
            }
            else if (!reg.test(phone)) { valPhoneMessage.style.display = "block"; $('#confirmValPhone').focus(); }
            else if (!IsEmail(mail)) { valMailMessage.style.display = "block"; $('#confirmValMail').focus(); }
            else {
                var orderId = $('#orderId').val();
                $('#orderTime').val(new Date().toJSON().slice(0, 19));

                // edit reservation order
                myAJAX(AjaxType.PUT, "/api/classorders/" + orderId, null, "application/json", JSON.stringify(GetFormData($('#editOrder'))))

                // edit classorder's classtimeID
                myAJAX(AjaxType.PUT, "/api/classrecords/" + crID, null, "application/json", JSON.stringify(classRecords));

                // edit visitor information
                var sId = $('#sId').val();
                myAJAX(AjaxType.PUT, "/api/students/" + sId, showMessage("已存檔"), "application/json", JSON.stringify(GetFormData($('#editForm'))))
            }
        })

        //open delete modal
        function opendeleteModel(orderID, classRecordID) {
            $('#delOrderID').val(orderID);
            $('#delClassRecordID').val(classRecordID);
            deleteModal.style.display = "block";
            $('#btnconfirmdelete').focus();
        }

        //select visitor
        function visitorSelect() {
            $('#visitorName').val("");
            $('#new_sID').val("");
            myAJAX(AjaxType.GET, "/api/students/guests/",showVisitorNameFromVisitorPhone)
        }
        function showVisitorNameFromVisitorPhone(data) {     
            var option;
            $('#visitorName').children("option").filter("[value != none]").remove();
            data.forEach(function (g) {
                if (g.sPhone == $('#visitorPhone').val().trim()) {
                    option = `<option value="${g.sId}">${g.sName}</option>`;
                    $('#visitorName').append(option);
                }
            })
            var defaultsID = $('#visitorName option:eq(1)').val();
            $('#new_sID').val(defaultsID);
        }
        function selectVisitor(x){
            var visitorID = x.value;
            $('#new_sID').val(visitorID);
        }

        //edit box select class name
        var selectClassNameValue;
        function changeClass(x) {
            selectClassNameValue = x.value;
            $('#classDate').empty();
            $('#new_classDate').children("option").filter("[value != none]").remove();        
            myAJAX(AjaxType.GET,"/api/classes/",selectorChange_ClassName);
        }
        function selectorChange_ClassName(data){
             data.forEach(function (c) {
                if (c.cName == selectClassNameValue) {
                            classID = c.cId;
                            $('#eId').val(c.eId);
                            $('#new_eId').val(c.eId);
                }
             })
            myAJAX(AjaxType.GET, "/api/classTimes/cId/" + classID, selectorChange_ClassDate)
        }
        function selectorChange_ClassDate(data){
            $('#classTimeId').val(data[0].classTimeId);
            data.forEach(function (t) {
                var option;
                var date = new Date(t.ctDate).toLocaleDateString();
                var weeday = t.weekday.substring(2);
                option = `<option value="${t.ctDate}">${date}(${weeday}) ${t.startTime}~${t.endTime}</option>`;
                $('#classDate').append(option);
                $('#new_classDate').append(option);
            })
        }

        //edit box select class date
        function changeDate(x) {
            var classDate = $('#classDate').val();
            var new_classDate = $('#new_classDate').val();
            myAJAX(AjaxType.GET, "/api/classTimes/cId/date/" + classID + "/" + classDate, putClassTimeIdValue);
            myAJAX(AjaxType.GET, "/api/classTimes/cId/date/" + classID + "/" + new_classDate, putClassTimeIdValue);
        }
        function putClassTimeIdValue(t){
            $('#classTimeId').val(t[0].classTimeId);
            $('#new_classTimeId').val(t[0].classTimeId);
        }

        //convert form data to JSON
        function GetFormData($form) {
            var unindexd_array = $form.serializeArray();
            var indexed_array = {};
            $.map(unindexd_array, function (n, i) {
                if (n['value'] == "false") { n['value'] = false; }
                else if (n['value'] == "true") { n['value'] = true; }
                else if (n['value'] == "on") {
                    if ($('input[name=sGender]')[0].checked == false) { n['value'] = false; } else { n['value'] = true; }
                }
                else if (n['value'] == "null"){
                    n['value'] = null;
                }
                indexed_array[n['name']] = n['value'];
            });
            return indexed_array;
}




   



