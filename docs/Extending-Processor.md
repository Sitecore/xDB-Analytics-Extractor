# Create a new Processor

## Introduction

This is a step-by-step tutorial to create a new processor. In this guide, we are going to create a `DeviceProcessor` that will transform data representing a device used to access the website. You can follow these instructions to create and use your custom processor. All you need to do is to change the core logic of the module to fit your needs.

## Overview

The tutorial is organized in sequential steps that can be outlined as follows:

1. [Create a new Model](#step-1-create-a-new-model)
2. [Define the model properties](#step-2-define-the-model-properties)
3. [Creating the Processor](#step-3-creating-the-processor)
4. [Adding the Processor logic](#step-4-adding-the-processor-logic)
5. [Registering your Model](#step-5-registering-your-model)
6. [Building your Model](#step-6-building-your-model)

> [!TIP]
> We recommend that you **read this tutorial in its entirety before actually beginning** with the procedure.
> Make sure to have understood everything in advance: this could save you some time and troubles later. This 
> procedure is rather foulproof (we hope), but you can never know.

## Step by Step Tutorial

### Step 1: Create a new Model

Models hold certain data about an interaction and help organise that data in a concept. For instance, a device model can hold different pieces of information from an interaction that together represent the idea of a device that was used to access the site. 

To create your new model, navigate to the `Models` folder and create a new `DeviceModel` class, and implement the `IModel` interface. The `IModel` interface is a **marker interface** used to mark a class as an exportable model.

```cs
public class DeviceModel : IModel 
{
}
```

If you want to use the database export functionality you also need to add the `Table` annotation, with the name of the table that stores the model's info.

> [!IMPORTANT]
> If you are extending the models and using the database export option of the service, you will need to update your database schema before running the processor. To do that follow [this]() guide.

```cs
[Table("Devices")]
public class DeviceModel : IModel 
{
}
```


> [!NOTE]
> Some models also extend the `TaxonModel` class. This class is used for models that contain taxonomy marketing data.
>  
> A taxonomy is a hierarchical structure that you can use to identify and organize information. In the Sitecore Experience Platform, you can use taxonomies to classify marketing activities, such as campaigns, goals, and events. You can apply taxonomy tags to these items in the Marketing Control Panel.
>
> For further reading please refer to the Sitecore documentation [here](https://doc.sitecore.com/xp/en/users/103/sitecore-experience-platform/marketing-taxonomies.html)

### Step 2: Define the model properties

As previously stated, a model is simply a container for grouping data to represent a specific entity. So we now need to add the model data.

```cs
public class DeviceModel : IModel
{
    public long Id { get; }
    public string? BrowserVersion { get; set; }
    public string? BrowserMajorName { get; set; }
    public string? BrowserMinorName { get; set; }
    public string? DeviceCategory { get; set; }
    public string? ScreenSize { get; set; }
    public string? OperatingSystem { get; set; }
    public string? OperatingSystemVersion { get; set; }
    public string? Language { get; set; }
    public bool? CanSupportTouchScreen { get; set; }
    public string? DeviceVendor { get; set; }
    public string? DeviceVendorHardwareModel { get; set; }
    public Guid? InteractionId { get; set; }
}
```

> [!IMPORTANT]  
> Every `IModel` class is required to contain a `public Guid? InteractionId { get; set; }` attribute. This attribute is used to associate the interaction with the model's specific data.

### Step 3: Creating the Processor

A processor is the part of the application that takes raw data from the xDB and changes it into a format the model can use. On the device model, for instance, the processor will change and combine data from three different facets to make the data for the device model.

To create your new processor, navigate to the `Processors` folder and create a new `DeviceProcessor` static class.  

```cs
public static class DeviceProcessor {

}
```

You must also define a `Process` method. This method must accept **the xDB arguments containing the raw data to be processed**. In our example we will use `WebVisit`, `UserAgentInfo` and `interactionId`. For the mapping of values in xDB please refer to the Sirecore documentation [here](https://doc.sitecore.com/xp/en/developers/92/sitecore-experience-platform/collection-model-reference.html).

```cs
public static class DeviceProcessor {
    public static DeviceModel? Process(WebVisit? webVisit, UserAgentInfo? userAgentInfo, Guid? interactionId)
    {
    }
}
```

### Step 4: Adding the Processor logic

You must now add your processor logic in this step. In essence, you must process the raw xdb data and populate the model you created in the previous steps, and return it.

```cs
public static class DeviceProcessor {
    public static DeviceModel? Process(WebVisit? webVisit, UserAgentInfo? userAgentInfo, Guid? interactionId)
    {
        if (webVisit is null || userAgentInfo is null || interactionId is null)
            return null;
    
        var browserData = webVisit?.Browser;
        var screenData = webVisit?.Screen;
        var operatingSystemData = webVisit?.OperatingSystem;

        return new DeviceModel()
        {
            BrowserMajorName = browserData?.BrowserMajorName ?? "null",
            BrowserMinorName = browserData?.BrowserMinorName ?? "null",
            BrowserVersion = browserData?.BrowserVersion ?? "null",
            OperatingSystem = operatingSystemData?.Name ?? "null",
            OperatingSystemVersion = operatingSystemData is null
                ? "null"
                : $"{operatingSystemData?.MajorVersion}.{operatingSystemData?.MinorVersion}",
            CanSupportTouchScreen = userAgentInfo?.CanSupportTouchScreen ?? false,
            DeviceCategory = userAgentInfo?.DeviceType ?? "null",
            DeviceVendor = userAgentInfo?.DeviceVendor ?? "null",
            DeviceVendorHardwareModel = userAgentInfo?.DeviceVendorHardwareModel ?? "null",
            InteractionId = interactionId,
            Language = webVisit?.Language ?? "null",
            ScreenSize = screenData is null
                ? "null"
                : $"{screenData?.ScreenWidth}x{screenData?.ScreenHeight}"
        };
    }
}
```

### Step 5: Registering your Model

You need to open the `Dto/InteractionDto.cs` class and add your model. If an interaction contains only a **single** instance of your model, then you should add it as is. However, if an interaction contains **multiple instances** of your model, then you need to provide a `List` of models.

```cs
public class InteractionDto {
    public DeviceModel? DeviceModel { get; init; } = new();

    // If the interaction contains multiple instances, use a List<T>
    public List<SearchModel> SearchModels { get; init; } = new List<SearchModel>();
}
```

### Step 6: Building your Model

In the final step, you need to use the newly created processor to build your model. To do that, navigate to the `Builder.SerializableObjectBuilder.cs` class and call your processor.

```cs
public static InteractionDto BuildInteractionDto(Interaction interaction)
{
    var dto = new InteractionDto
    {
        DeviceModel = DeviceProcessor.Process(interaction.WebVisit(), interaction.UserAgentInfo(), interaction.Id)
    }
}
```

After doing that, you need to rebuild and register the application using the installation scripts. For a quick overview of the actions available, refer to the [Getting Started](../README.md) guide.