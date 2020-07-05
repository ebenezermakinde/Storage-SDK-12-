# Store our variables
$resoureGroupName="<ResoureGroupName>"
$location="westus2"
$storageAccountName = "<storageAccountName>"

# Configure defaults
az configure --defaults `
location=$location `
group=$resoureGroupName

# Create resource group
az group create -n $resoureGroupName

# Create Storage account
az storage account create -n $storageAccountName --sku Standard_LRS --kind StorageV2

# Show storage connection string
az storage account show-connection-string -n $storageAccountName

# Create a Container in the Storage account
az storage container create -n testcontainer --account-name $storageAccountName
