
var data = [];
var ids = '';

$(function () {

    FillArray();


    $('#tagcomment').mentionsInput({
        onDataRequest: function (mode, query, callback) {
      // data = [
      //  { id:1, name:'Kenneth Auchenberg', 'avatar':'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'type':'contact' },
      //  { id:2, name:'Jon Froda', 'avatar':'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'type':'contact' },
      //  { id:3, name:'Anders Pollas', 'avatar':'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'type':'contact' },
      //  { id:4, name:'Kasper Hulthin', 'avatar':'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'type':'contact' },
      //  { id:5, name:'Andreas Haugstrup', 'avatar':'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'type':'contact' },
      //  { id:6, name:'Pete Lacey', 'avatar':'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'type':'contact' },
      //  { id:7, name:'kenneth@auchenberg.dk', 'avatar':'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'type':'contact' },
      //  { id:8, name:'Pete Awesome Lacey', 'avatar':'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'type':'contact' },
      //  { id:9, name:'Kenneth Hulthin', 'avatar':'http://cdn0.4dots.com/i/customavatars/avatar7112_1.gif', 'type':'contact' }
      //];
        FillArray();
        data = _.filter(data, function (item) { return item.name.toLowerCase().indexOf(query.toLowerCase()) > -1 });

      callback.call(this,data);
    }
  });

    $('.get-syntax-text').click(function () {
        
        $('#tagcomment').mentionsInput('val', function (text) {
      alert(text);
    });
  });

  //$('#btnsendcomment').click(function () {
  //    $('#txtcomment').mentionsInput('getMentions', function (data) {
  //       // 
  //        //var ids = '';
  //        $.each(data,function (i,d) {

  //            //var i = data[i].name;
  //            ids = ids.concat(data[i].id + ',');

  //            alert(ids);

              


  //        })

  //        //GetIds(ids);
  //     // alert(JSON.stringify(data));
  //      //alert(data);
  //  });
  //}) ;

});



function GetIds(ids)
{
    $.ajax({
        //contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        url: '../Home/GetIds',
        data: { ids:ids},
        success: function (dbdata) {
            
          
        },
        error: function () {

        }
    });
}





function FillArray() {
    $.ajax({
        //contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'Get',
        url: '../Comment/GetUsers',
        //data: { ids: ids },
        success: function (dbdata) {
            
           // 
            data = dbdata;
            

        },
        error: function () {

        }
    });
}
