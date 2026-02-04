// generate a plot with D3.js of the selling price of the album by year
// x-axis are the month series and y-axis show the numbers of albums sold
// data from the sales of album are loaded in from an external source and are in json format
import * as d3 from "d3";

// Function to generate a bar plot of album sales by month
export function generateAlbumSalesPlot(
  selector: string,
  data: { month: string; sales: number }[],
) {
  const margin = { top: 20, right: 30, bottom: 40, left: 40 };
  const width = 800 - margin.left - margin.right;
  const height = 400 - margin.top - margin.bottom;
  const svg = d3
    .select(selector)  // Use the selector parameter instead of "body"
    .append("svg")
    .attr("width", width + margin.left + margin.right)
    .attr("height", height + margin.top + margin.bottom)
    .append("g")
    .attr("transform", `translate(${margin.left},${margin.top})`);
  const x = d3
    .scaleBand()
    .domain(data.map((d) => d.month))
    .range([0, width])
    .padding(0.1);
  const y = d3
    .scaleLinear()
    .domain([0, d3.max(data, (d) => d.sales)!])
    .nice()
    .range([height, 0]);
  svg
    .append("g")
    .attr("class", "x-axis")
    .attr("transform", `translate(0,${height})`)
    .call(d3.axisBottom(x));
  svg.append("g").attr("class", "y-axis").call(d3.axisLeft(y));
  svg
    .selectAll(".bar")
    .data(data)
    .enter()
    .append("rect")
    .attr("class", "bar")
    .attr("x", (d) => x(d.month)!)
    .attr("y", (d) => y(d.sales))
    .attr("width", x.bandwidth())
    .attr("height", (d) => height - y(d.sales))
    .attr("fill", "steelblue");
}
// Example usage:
// const salesData = [
//   { month: "January", sales: 150 },
//   { month: "February", sales: 200 },
//   { month: "March", sales: 250 },
//   { month: "April", sales: 300 },
//   { month: "May", sales: 280 },
//   { month: "June", sales: 350 },
//   { month: "July", sales: 400 },
//   { month: "August", sales: 420 },
//   { month: "September", sales: 380 },
//   { month: "October", sales: 450 },
//   { month: "November", sales: 480 },
//   { month: "December", sales: 500 },
// ];
// generateAlbumSalesPlot("body", salesData);
