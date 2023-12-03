const fs = require('fs');

var input = fs.readFileSync('./day3input1.txt').toString('utf-8');

input = input.split('\r\n');

let sum = 0;
let gears = {};

for (let i = 0; i < input.length; i++)
{
    let searchLine = JSON.parse(JSON.stringify(input[i])); //make deep copy
    let lineLength = searchLine.length;
    while (searchLine.search(/[0-9]/) != -1)
    {
        let startIndex = searchLine.search(/[0-9]/);
        searchLine = searchLine.substring(startIndex);
        let endIndex = searchLine.search(/[^0-9]/);
        let number;
        if (endIndex == -1)
        {
            number = searchLine;
            searchLine = "";

            if (CheckSurrounding(i, lineLength - number.length, number.length, number)) sum += parseInt(number);
            continue;
        }
        else
        {
            number = searchLine.substring(0, endIndex);
            searchLine = searchLine.substring(endIndex);
        }
        if (CheckSurrounding(i, lineLength - searchLine.length - endIndex, number.length, number)) sum += parseInt(number);
    }
}

let gearSum = 0;
Object.keys(gears).forEach(key => {
    if (gears[key].summate)
        gearSum += gears[key].value
})
console.log(`Part 1 Answer: ${sum} | Part 2 Answer: ${gearSum}`);


//Util Methods
function CheckSurrounding(line, lineIndex, length, number)
{
    for (let i = 0; i < length; i++)
    {
        let currentIndex = lineIndex + i;
        //check line above
        if (line != 0)
        {
            if (currentIndex > 0)
                if (CheckIndex(line -1, currentIndex -1, number))
                    return true;

            if (CheckIndex(line - 1, currentIndex, number))
                return true;
            if (currentIndex < input[line - 1].length -1)
                if (CheckIndex(line - 1, currentIndex + 1, number)) 
                    return true;
        }
        //Check current line
        if (currentIndex > 0)
                if (CheckIndex(line, currentIndex -1, number)) 
                    return true;

        if (currentIndex < input[line].length -1)
            if (CheckIndex(line, currentIndex + 1, number)) 
                return true;

        //Check line below
        if (line != input.length - 1)
        {
            if (currentIndex > 0)
                if (CheckIndex(line + 1, currentIndex -1, number)) 
                    return true;
            if (CheckIndex(line + 1, currentIndex, number)) 
                return true;
            if (currentIndex < input[line + 1].length -1)
                if (CheckIndex(line + 1, currentIndex + 1, number)) 
                    return true;
        }
    }
    return false;
}


function CheckIndex(i, j, number)
{
    if (input[i][j].search(/[^0-9\.]/) != -1) 
    {
        if (input[i][j] == "*") 
        {
            if (gears.hasOwnProperty(`x${j}y${i}`))
            {
                gears[`x${j}y${i}`].summate = true;
                gears[`x${j}y${i}`].value *= parseInt(number);
            }
            else
            {
                gears[`x${j}y${i}`] = {};
                gears[`x${j}y${i}`].value = parseInt(number);
            }
        }
        return true;
    }
    return false;
}