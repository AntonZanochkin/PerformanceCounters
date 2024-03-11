import _ from "lodash";

export const vegaLiteSpec = {
  schema: "https://vega.github.io/schema/vega-lite/v5.json",
  data: { name: "counterData" },
  vconcat: [
    {
      layer: [
        {
          mark: { type: "bar" },
          encoding: {
            x: {
              timeUnit: "binnedminutes",
              field: "dateTime",
              type: "temporal",
              axis: {
                title: "Time",
                labelAngle: 90,
                format: "%H:%M  (%d %b)",
                color: { value: "grey" },
                gridColor: "#565656",
                gridOpacity: 0.5,
                titleColor: "#a1a1af",
                titleFontSize: 16,
                tickColor: "white",
                tickLabelColor: "white",
                tickLabelFontSize: 5,
                tickSize: 16,
                labelColor: "#cccccc",
                labelFontSize: 16,
              },
              scale: { domain: { param: "brush" } },
            },
            y: {
              field: "value",
              type: "quantitative",
              //aggregate: "sum",
              axis: {
                title: "Value",
                gridColor: "#565656",
                gridOpacity: 0.5,
                titleColor: "#a1a1af",
                titleFontSize: 16,
                labelColor: "#cccccc",
                labelFontSize: 16,
              },
            },
            tooltip: [
              {
                field: "dateTime",
                title: "DateTime",
                type: "temporal",
                format: "%H:%M %p (%d %b)",
              },
              {
                field: "value",
                title: "Value",
                type: "quantitative",
              },
            ],
          },
        },
      ],
    },
    {
      mark: "bar",
      height: 60,
      encoding: {
        x: {
          timeUnit: "binnedminutes",
          field: "dateTime",
          type: "temporal",
          title: "Datetime",
          axis: {
            title: "Time",
            labelAngle: 90,
            format: "%H:%M  (%d %b)",
            title: { color: "blue" },
            color: {
              value: "grey",
            },
            gridColor: "#565656",
            gridOpacity: 0.5,
            titleColor: "#a1a1af",
            titleFontSize: 16,
            tickColor: "white",
            tickLabelColor: "white",
            tickLabelFontSize: 16,
            tickSize: 10,
            labelColor: "#cccccc",
            labelFontSize: 12,
          },
        },
        y: {
          field: "value",
          type: "quantitative",
          title: "Time",
          axis: {
            title: "Value",
            tickCount: 3,
            color: {
              value: "grey",
            },
            gridColor: "#565656",
            gridOpacity: 0.5,
            titleColor: "#a1a1af",
            titleFontSize: 16,
            tickColor: "white",
            tickLabelColor: "white",
            tickLabelFontSize: 16,
            tickSize: 10,
            labelColor: "#cccccc",
            labelFontSize: 12,
          },
        },
      },
      params: [
        {
          name: "brush",
          encodings: ["x"],
          select: {
            type: "interval",
            encodings: ["x"],
            mark: { fill: "#fdbb84", fillOpacity: 0.3, stroke: "#e34a33" },
          },
        },
      ],
    },
  ],
  background: "#2f2d2d",
};

const buildSpec = (title) => {
  var vegaLiteSpecClone = _.cloneDeep(vegaLiteSpec);
  vegaLiteSpecClone.vconcat[0].layer[0].encoding.y.axis.title = title;
  vegaLiteSpecClone.vconcat[1].encoding.y.axis.title = title;
  return vegaLiteSpecClone;
};

export const integerSpec = () => {
  const spec = buildSpec("Value");
  return spec;
};

export const stopwatchSpec = () => {
  const spec = buildSpec("Seconds");
  return spec;
};

export const cpuTimeSpec = () => {
  const cpuTimeSpec = buildSpec("Seconds");
  cpuTimeSpec.vconcat[0].layer[0].encoding.color = {
    field: "timeType",
    legend: {
      title: "Time Type",
      titleColor: "white",
      titleFontSize: 18,
      labelColor: "#a1a1af",
      labelFontSize: 16,
    },
  };

  cpuTimeSpec.vconcat[1].encoding.color = {
    field: "timeType",
    legend: {
      title: "Time Type",
      titleColor: "white",
      titleFontSize: 18,
      labelColor: "#a1a1af",
      labelFontSize: 16,
    },
  };

  return cpuTimeSpec;
};
