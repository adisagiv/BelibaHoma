function fixHighchartMenu() {
    var options = Highcharts.getOptions();

    options.exporting.buttons.contextButton.menuItems.splice(0, 2);

    // remove download as svg
    options.exporting.buttons.contextButton.menuItems.pop();
    //remove doenload as pdf
    options.exporting.buttons.contextButton.menuItems.pop();
}

fixHighchartMenu();