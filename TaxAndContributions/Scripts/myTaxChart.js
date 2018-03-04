drawTax = function (netSalary, tax20, tax40, tax45, nationalInsurance, pension, ccVouchers, studentLoan) {

    var taxData = [parseFloat(netSalary), parseFloat(tax20), parseFloat(tax40), parseFloat(tax45), parseFloat(nationalInsurance), parseFloat(pension), parseFloat(ccVouchers), parseFloat(studentLoan)]
    //access chart element
    const ctx1 = document.getElementById("taxChart");
    //var taxData = [1, 2, 3, 4, 5, 6, 7, 8, 9];
    var taxLabels = ["Net salary", "Tax at 20%", "Tax at 40%", "Tax at 45%", "National Insurance", "Pension contribution", "Childcare vouchers", "Student loan repayment"];
    //set up data
    data = {
        datasets: [{
            data: taxData,
            backgroundColor: [
                   'rgba(0, 0, 255, 0.5)',
                   'rgba(255, 0, 0, 0.2)',
                   'rgba(255, 0, 0, 0.5)',
                   'rgba(255, 0, 0, 0.8)',
                   'rgba(255, 255, 0, 0.5)',
                   'rgba(255, 255, 255, 0.5)',
                   'rgba(120, 0, 255, 0.5)',
                   'rgba(255, 0, 104, 0.5)']
        }],

        // These labels appear in the legend and in the tooltips when hovering different arcs
        labels: taxLabels


    };

    //draw chart
    var taxChart = new Chart(ctx1, {
        type: 'pie',
        data: data,
        options: {
            title: {
                display: true,
                fontFamily: "'Helvetica Neue', 'Helvetica', 'Arial', sans-serif",
                fontSize: 18,
                fontStyle: 'bold',
                text: 'Net salary, tax and other contributions'
            },
            legend: {
                display: false,
                labels: {
                    fontColor: 'rgb(255, 99, 132)'
                }
            }
        }
    });
}
    //data: {
    //    labels: ["Red", "Blue", "Yellow", "Green", "Purple", "Orange"],
    //    datasets: [{
    //        label: '# of Votes',
    //        data: [12, 19, 3, 5, 2, 3],
    //        backgroundColor: [
    //            'rgba(255, 99, 132, 0.2)',
    //            'rgba(54, 162, 235, 0.2)',
    //            'rgba(255, 206, 86, 0.2)',
    //            'rgba(75, 192, 192, 0.2)',
    //            'rgba(153, 102, 255, 0.2)',
    //            'rgba(255, 159, 64, 0.2)'
    //        ],
    //        borderColor: [
    //            'rgba(255,99,132,1)',
    //            'rgba(54, 162, 235, 1)',
    //            'rgba(255, 206, 86, 1)',
    //            'rgba(75, 192, 192, 1)',
    //            'rgba(153, 102, 255, 1)',
    //            'rgba(255, 159, 64, 1)'
    //        ],
    //        borderWidth: 1
    //    }]
    //},
    //options: {
    //    scales: {
    //        yAxes: [{
    //            ticks: {
    //                beginAtZero: true
    //            }
    //        }]
    //    }
    //}
//});