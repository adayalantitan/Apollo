function getWebServiceUrlBase(context){
    switch (context){
        case "contracts":
        case "dlContracts":
            return '/services/IOService.asmx/GetContractsGrid';
        case "audits":
            return '/services/IOService.asmx/GetAuditGrid';
        default:
            return '../services/IOService.asmx/GetGrid';
    }    
}
function getPostParams(useContext,filterObject){
    var postParams;
    switch(useContext){
        case "contracts":
        case "dlContracts":
            postParams = {companyId:filterObject.filters[0].id,
                marketId:filterObject.filters[1].id,
                profitCenterId:filterObject.filters[2].id,
                mediaTypeId:filterObject.filters[3].id,
                mediaFormId:filterObject.filters[4].id,
                panelSubId:filterObject.filters[5].id,
                program:filterObject.filters[6].id,
                advertiserId:filterObject.filters[7].id,
                agencyId:filterObject.filters[8].id,
                aeId:filterObject.filters[9].id,
                salesMarketId:filterObject.filters[10].id        
            };
            break;
        case "audits":
            postParams = {fromDate:filterObject.filters[0].value,toDate:filterObject.filters[1].value};
            break;
        default:
            postParams = {context:useContext,
                companyId:filterObject.filters[0].id,
                marketId:filterObject.filters[1].id,
                profitCenterId:filterObject.filters[2].id,
                mediaTypeId:filterObject.filters[3].id,
                mediaFormId:filterObject.filters[4].id,
                parentProductClassId:filterObject.filters[5].id
            };
            break;
    }
    return postParams;
}
function getReturnValColName(context){    
    switch(context){
        case "advertiser":
        case "agency":
        case "consolidatedAdvertiser":
        case "consolidatedAgency":
        case "customer":
        case "consolidatedCustomer":
            return 'cusId';                    
        case "dlContracts":            
        case "contracts":
            return 'contractNumber';
        case "ae":
            return 'aeId';
        case "mediaForm":
            return 'mediaFormId';
        case "productClass":
            return 'productClassId';
        case "station":
            return 'stationId';   
        default:
            return null;
    }
}
function getReturnTextColNames(context){    
    switch(context){
        case "advertiser":
        case "agency":         
        case "consolidatedAdvertiser":
        case "consolidatedAgency":         
        case "customer":
        case "consolidatedCustomer":
            return ['cusName'];                          
        case "dlContracts":            
        case "contracts":
            return ['contractNumber','coId'];
        case "ae":
            return ['aeName'];        
        case "mediaForm":
            return ['mediaFormDescription'];
        case "productClass":
            return ['productClassDesc'];
        case "station":
            return ['station'];
        default:
            return null;
    }
}
function getColNames(context){    
    switch(context){
        case "advertiser":            
            return ['Advertiser Name','Advertiser ID','Contact Name','Customer Type','Company ID','Company ID'];            
        case "consolidatedAdvertiser":            
            return ['Advertiser Name','Advertiser ID','Company',''];
        case "agency":            
            return ['Agency Name','Agency ID','Contact Name','Customer Type','Company ID','Company ID'];
        case "consolidatedAgency":            
            return ['Agency Name','Agency ID','Company',''];
        case "customer":
            return ['Customer Name','Customer ID','Company','Company ID'];
        case "consolidatedCustomer":
            return ['Consolidated Customer Name','Consolidated Customer ID','Company','Company ID'];
        case "audits":
            return ['Search On','Table Key','Table Name','Table Name Raw','Field Name','Value Changed From','Value Changed To','Date Changed','Changed By','Action Taken'];
        case "contracts":
        case "dlContracts":
            return ['Contract', 'Contract', 'Advertiser', 'Agency', 'AE', 'Program', 'Start Date', 'End Date', 'Company Id'];            
        case "ae":
            return ['AE ID','AE Name','Company ID','','',''];        
        case "mediaForm":
            return ['Media Type Id','Media Type','Media Form','Media Form Description','Active','Company ID'];
        case "productClass":
            return ['Product Class Id', 'Parent Product Class Id', 'Parent Code', 'Parent Product Class', 'Product Class Code', 'Product Class'];
        case "station":
            return ['Market Id','Market','Station Group','Station Id','Station']
        default:
            return null;
    }
}
function getColModelObject(context){    
    switch(context){
        case "advertiser":
        case "agency":
            return [                                
                {name:'cusName',index:'CUSTOMER_NAME',sort:true,width:250},
                {name:'cusId',index:'CUSTOMER_ID',sort:true,width:100,align:'center'},                
                {name:'contact',index:'CONTACT_NAME',search:false,sort:true},                
                {name:'cusType',index:'CUSTOMER_CODE',hidden:true,search:false},
                {name:'coId',index:'COMPANY_ID',hidden:true,search:false},
                {name:'coName',index:'COMPANY_NAME',search:false,sort:false,align:'center'}
            ];
        case "consolidatedAdvertiser":
        case "consolidatedAgency":  
        case "consolidatedCustomer":
        case "customer":                 
            return [                
                {name:'cusName',index:'CUSTOMER_NAME',sort:true,width:250},                
                {name:'cusId',index:'CUSTOMER_ID',sort:true,width:100,align:'center'},
                {name:'coName',index:'COMPANY_NAME',search:false,sort:false,align:'center'},
                {name:'coId',index:'COMPANY_ID',hidden:true,search:false}                
            ];
        case "contracts":
        case "dlContracts":
            return [{ name: 'contractNumber', index: 'CONTRACT_NUMBER', width: 75, hidden: true },
                { name: 'contractNumberLink', index: 'CONTRACT_NUMBER_LINK', width: 75 },
                { name: 'advertiser', index: 'ADVERTISER', sort: true, search: false, width: 200 },
                { name: 'agency', index: 'AGENCY', sort: true, search: false, width: 200 },
                { name: 'ae1', index: 'AE_1_NAME', sort: true, width: 110, search: false },
                { name: 'program', index: 'PROGRAM', sort: true, width: 100, search: false },
                { name: 'startDate', index: 'CONTRACT_START_DATE', sort: true, align: 'right', width: 100, formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y'} },
                { name: 'endDate', index: 'CONTRACT_END_DATE', sort: true, align: 'right', width: 100, formatter: 'date', formatoptions: { srcformat: 'm/d/Y H:i:s', newformat: 'm/d/Y'} },
                { name: 'companyId', index: 'COMPANY_ID', hidden: true }
            ];
            case "ae":
                return [{ name: 'aeId', index: 'ACCOUNT_EXECUTIVE_ID', sort: true, width: 75, align: 'center' },
                { name: 'aeName', index: 'ACCOUNT_EXECUTIVE_NAME', sort: true, width: 200 },
                { name: 'coName', index: 'COMPANY_NAME', search: false, sort: false, align: 'center' },
                { name: 'coId', index: 'COMPANY_ID', hidden: true, search: false },
                { name: 'active', index: 'ACTIVE', hidden: true, search: false },
                { name: 'activeAeId', index: 'ACTIVE_AE_ID', hidden: true, search: false }
            ];
            case "audits":
                return [{ name: 'searchOn', index: 'SEARCH_ON', sort: true },
                { name: 'tableKey', index: 'TABLE_KEY', hidden: true, search: false },
                { name: 'tableNameDisplay', index: 'TABLE_NAME_DISPLAY', sort: true, stype: 'select', editoptions: { value: ":ALL;AE_DRAW_PAYMENT:AE Draw/Payment;ACCOUNT_EXECUTIVE_FLAT_RATE:AE Flat Rate;AE_REPORTING:AE Reporting;COMMISSION_SPLIT_PERCENTAGE_USA:Commission Allocations (USA);COMMISSION_SPLIT_PERCENTAGE_CAN:Commission Allocations (Canada);RATE:Commission Rates;RATE_DETAIL:Commission Rate Detail;v_CUSTOMER_XREF:Customer Rollup;COMMISSION_PROD_OVERRIDE:Production Commission Override"} },
                { name: 'tableName', index: 'TABLE_NAME', hidden: true, search: false },
                { name: 'fieldName', index: 'FIELD_NAME', sort: true, search: false },
                { name: 'fromValue', index: 'FROM_VALUE', sort: true, search: false },
                { name: 'toValue', index: 'TO_VALUE', sort: true, search: false },
                { name: 'dateChanged', index: 'DATE_CHANGED', sort: true, search: false, align: 'right', width: 100 },
                { name: 'changedBy', index: 'CHANGED_BY', sort: true, search: false },
                { name: 'actionTaken', index: 'ACTION_TAKEN', sort: true, align: 'center', width: 65, stype: 'select', editoptions: { value: ":ALL;Add:Add;Update:Update;Delete:Delete"} }
            ];
            case "mediaForm":
                return [{ name: 'mediaTypeId', index: 'MEDIA_TYPE_ID', hidden: true, search: false },
                { name: 'mediaTypeDescription', index: 'MEDIA_TYPE_DESCRIPTION', sort: true, align: 'center', width: 100, stype: 'select', editoptions: { value: ":ALL;BAN:Banner;BB:Billboard;B:Bulletin;BUS:Bus;C:Cone;D:Digital;Kiosk:Kiosk;MAR:Marketing;P:Posters;RAIL:Rail;STA:Station;S:Storage;SUB:Subway"} },
                { name: 'mediaFormId', index: 'MEDIA_FORM_ID', hidden: true, search: false },
                { name: 'mediaFormDescription', index: 'MEDIA_FORM_DESCRIPTION', sort: true, width: 300 },
                { name: 'active', index: 'ACTIVE', hidden: true, search: false },
                { name: 'companyId', index: 'COMPANY_ID', hidden: true, search: false }
            ];
            case "productClass":
                return [{ name: 'productClassId', index: 'PRODUCT_CLASS_ID', hidden: true, search: false },
                { name: 'parentProductClassId', index: 'PARENT_ID', hidden: true, search: false },
                { name: 'parentCode', index: 'PARENT_CODE', hidden: true, search: false },
                { name: 'parentDesc', index: 'PARENT_DESCRIPTION', sort: true, align: 'center', width: 175 },
                { name: 'productClassCode', index: 'PRODUCT_CLASS_CODE', hidden: true, search: false },
                { name: 'productClassDesc', index: 'PRODUCT_CLASS_DESCRIPTION', sort: true, align: 'center', width: 200 },
            ];
            case "station":
                return [{ name: 'marketId', index: 'MARKET_CODE', hidden: true, search: false },
                { name: 'market', index: 'MARKET_DESCRIPTION', sort: true, align: 'center', width: 100 },
                { name: 'stationGroup', index: 'STATION_GROUP', sort: true, align: 'center', width: 125 },
                { name: 'stationId', index: 'STATION_ID', hidden: true, search: false },
                { name: 'station', index: 'STATION_NAME', sort: true, align: 'center', width: 300 }
            ];
        default:
            return null;
    }
}
function BuildFilterObjectByContext(context, displayVal, displayId, companyVal, companyId) {
    switch (context) {
        case "advertiser":
        case "agency":
        case "consolidatedAdvertiser":
        case "consolidatedAgency":
        case "ae":
            return BuildFilterObject(displayVal, displayId, '', '', '', '', '', '', '', '', '', '');
        case "mediaForm":
            return BuildFilterObject(companyVal, companyId, '', '', '', '', displayVal, displayId, '', '', '', '');
        case "productClass":
            return BuildFilterObject(companyVal, companyId, '', '', '', '', '', '', '', '', displayVal, displayId);
        case "station":
            return BuildFilterObject('', '', displayVal, displayId, '', '', '', '', '', '', '', '');
    }
}
function BuildFilterObject(company, companyId, market, marketId, profitCenter, profitCenterId, mediaType, mediaTypeId, mediaForm, mediaFormId, parentProductClass, parentProductClassId) {
    var filterObject = {
        'filters': [{ name: 'company', displayVal: company, id: companyId },
            { name: 'market', displayVal: market, id: marketId },
            { name: 'profitCenter', displayVal: profitCenter, id: profitCenterId },
            { name: 'mediaType', displayVal: mediaType, id: mediaTypeId },
            { name: 'mediaForm', displayVal: mediaForm, id: mediaFormId },
            { name: 'parentProductClassId', displayVal: parentProductClass, id: parentProductClassId }
        ]
    };
    return filterObject;
}
function BuildContractFilterObject(companyId, marketId, profitCenterId, mediaTypeId, mediaFormId, panelSubId, program, advertiserId, agencyId, aeId, salesMarketId) {
    var filterObject = {
        'filters': [{ name: 'company', id: companyId },
            { name: 'market', id: marketId },
            { name: 'profitCenter', id: profitCenterId },
            { name: 'mediaType', id: mediaTypeId },
            { name: 'mediaForm', id: mediaFormId },
            { name: 'panelSub', id: panelSubId },
            { name: 'program', id: program },
            { name: 'advertiser', id: advertiserId },
            { name: 'agency', id: agencyId },
            { name: 'ae', id: aeId },
            { name: 'salesMarket', id: salesMarketId },
        ]
    };
    return filterObject;
}
function BuildAuditFilterObject(fromDate,toDate){
    var filterObject = {'filters' : [{name:'fromDate',value:fromDate},{name:'toDate',value:toDate}]};
    return filterObject;
}
function UpdateStaticGrid(context,gridId,filterObject){
    var postParams = getPostParams(context,filterObject);
    $('#'+gridId).setGridParam({postData:postParams}).trigger('reloadGrid');
}
function getReturnValue(rowData,colName){                    
    return trimValue(rowData[colName]);    
}
function getReturnText(rowData,colNames,sep){                
    var returnVal = '';    
    for (var col in colNames){
        returnVal += ((returnVal=='')? ' ' : (' ' + sep + ' ')) + rowData[colNames[col]];
    }
    return trimValue(returnVal);
}
function showGrid(context,webServiceUrl,filterObject,targetValueControl,targetTextControl){
    var colNames = getColNames(context);
    var colModelObject = getColModelObject(context);
    var returnValCol = getReturnValColName(context);
    var returnTextColNames = getReturnTextColNames(context);
    var postParams = getPostParams(context,filterObject);
    try {
        var theGrid = $('#globalGrid').jqGrid({
            //url:'/services/IOService.asmx/GetAdvertiserGridx'
            url: webServiceUrl,
            postData: postParams,
            datatype: 'xml',
            height: 350,
            autowidth:true,
            colNames:colNames,
            colModel:colModelObject,
            rowNum:25,            
            rowList:[25,50,100],
            pager: '#globalGridPager',
            viewrecords: true,
            toolbar: [true,'top'],            
            gridview: true,
            onSelectRow: function(id){
                var rowData = $('#globalGrid').getRowData(id);
                targetValueControl.value=getReturnValue(rowData,returnValCol);
                targetTextControl.value=getReturnText(rowData,returnTextColNames,'-');
                $get(globalGridCloseButtonId).click();
                //When a selection is made, make sure the grid is unloaded
                $('#globalGrid').GridUnload('globalGrid');
            }
        })                        
        .navGrid('#globalGridPager',{edit:false,add:false,del:false,search:false,refresh:false})                        
        .navButtonAdd('#globalGridPager',{caption:"Clear Search",title:"Clear Search",buttonicon:'ui-icon-refresh',
            onClickButton:function(){theGrid[0].clearToolbar();}
        });                                                
        theGrid.filterToolbar();        
        var postData = theGrid.getPostData();
    } catch(e) {} 
}    
function showGridWithCallback(context,gridId,pagerId,webServiceUrl,filterObject,selectionCallback){
    var colNames = getColNames(context);
    var colModelObject = getColModelObject(context);    
    var postParams = getPostParams(context,filterObject);      
    try {
        var theGrid = $('#'+gridId).jqGrid({            
            url: webServiceUrl,
            postData: postParams,
            datatype: 'xml',
            height: 350,
            autowidth:true,
            colNames:colNames,
            colModel:colModelObject,
            rowNum:25,            
            rowList:[25,50,100],
            pager: '#'+pagerId,
            viewrecords: true,
            toolbar: [true,'top'],            
            gridview: false,
            onSelectRow: selectionCallback,
            afterInsertRow: BuildLink
        })                        
        .navGrid('#'+pagerId,{edit:false,add:false,del:false,search:false,refresh:false})                        
        .navButtonAdd('#'+pagerId,{caption:"Clear Search",title:"Clear Search",buttonicon:'ui-icon-refresh',
            onClickButton:function(){theGrid[0].clearToolbar();}
        });                                                
        theGrid.filterToolbar();
        var postData = theGrid.getPostData();
    } catch(e) {} 
}
function DisplaySearchGrid(context,targetValueControl,targetTextControl,filterObject){    
    var webServiceUrl = getWebServiceUrlBase(context);    
    showGrid(context,webServiceUrl,filterObject,targetValueControl,targetTextControl);    
    $find('globalGridPopupExtBehavior').show();
}
function DisplayStaticGrid(context,gridId,pagerId,filterObject,selectionCallback){
    var webServiceUrl = getWebServiceUrlBase(context);
    showGridWithCallback(context,gridId,pagerId,webServiceUrl,filterObject,selectionCallback);    
}
function BuildLink(rowid,rowdata,rowelem){
    var contractNumber = rowdata["contractNumber"];
    var companyId = rowdata["companyId"];
    var link = "<a href='#' onclick='ShowContractDetail(" + contractNumber + "," + companyId + ");'>" + contractNumber + "</a>";
    $('.contractSearchGridTable').setCell(rowid, 1, link);
}