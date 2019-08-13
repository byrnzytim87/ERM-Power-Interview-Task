'use strict';
console.log('Loading ERM Power Request Handler');

const AWS = require('aws-sdk');
const docClient = new AWS.DynamoDB.DocumentClient({region: 'ap-southeast-2'});

exports.handler = (event) => {
    
    let dataValues = [];
    var params;
    
    

    if(event[0].FileType === "LP")
    {
        event.forEach(item => {
        dataValues.push(
                    item['DataValue']
        )
        });
    
        // Store LP data as a json blob, with energy median, max and min in separate columns
        params = {
            Item: {
                ID: createGuid().toString(),
                data: JSON.stringify(event),
                median: calculateMedian(dataValues),
                max: calculateMax(dataValues),
                min: calculateMin(dataValues)
            },
            TableName: 'ERMPowerLP'
        };
    }
    
    else if(event[0].FileType === "TOU")
    {
        event.forEach(item => {
        dataValues.push(
                    item['Energy']
        )
        });
        // Store TOU data as a json blob, with dataValue median, max and min in separate columns
        params = {
            Item: {
                ID: createGuid().toString(),
                data: JSON.stringify(event),
                median: calculateMedian(dataValues),
                max: calculateMax(dataValues),
                min: calculateMin(dataValues)
            },
            TableName: 'ERMPowerTOU'
        };
    }
    
    docClient.put(params, function(error, data){
        if(error){}
        else{}
    });
    
    // Code to store data in appropriate columns, rather than a json blob
    
    //let records = [];
    
    // event.forEach(item => {
    //     records.push({
    //         PutRequest: {
    //             Item: {
    //                 ID: createGuid().toString(),
    //                 metercode: item['MeterPointCode'],
    //                 serialnumber: item['SerialNumber'],
    //                 plantcode: item['PlantCode'],
    //                 date: item['Date'],
    //                 datatype: item['DataType'],
    //                 datavalue: item['DataValue'],
    //                 units: item['Units'],
    //                 data: JSON.stringify(item)
    //             }
    //         }
    //     });
    // });
    
    // let params = {
    //     RequestItems: {
    //         'ERMPower' : records
    //     }
    // };
        
    // docClient.batchWrite(params, function(err, data) {
    //     if (err) console.log(err, err.stack);
    //     else     console.log(data);
    // });
    
    //docClient.batchWrite();
    
    function calculateMax(array) {
        return Math.max.apply(null, array);
    }
    
    function calculateMin(array) {
        return Math.min.apply(null, array);
    }
    
    function calculateMedian(values){
      if(values.length ===0) return 0;
    
      values.sort(function(a,b){
        return a-b;
      });
    
      var half = Math.floor(values.length / 2);
    
      if (values.length % 2)
        return values[half];
    
      return (values[half - 1] + values[half]) / 2.0;
    }
    
    function createGuid() {
      return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
      });
    }

    let responseCode = 200;
    let body = '';
    console.log("request: " + JSON.stringify(event));

    let responseBody = {
        message: body + 'testBody1',
        input: event
    };

    let response = {
        statusCode: responseCode,
        headers: {
            "x-custom-header" : "my custom header value"
        },
        body: responseBody
    };
    

    console.log("response: " + JSON.stringify(response))
    return response;
};
