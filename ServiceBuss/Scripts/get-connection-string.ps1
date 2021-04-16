az servicebus namespace authorization-rule keys list 
    --resource-group test-learning-stuff 
    --name RootManageSharedAccessKey 
    --query primaryConnectionString 
    --output tsv 
    --namespace-name servicebus-learn-paths-salesteamapp