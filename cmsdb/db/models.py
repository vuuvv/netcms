from django.db import models

from mptt.models import MPTTModel, TreeForeignKey

# Create your models here.

class Page(MPTTModel):
	title = models.CharField(max_length=100)
	slug = models.CharField(max_length=255)
	parent = TreeForeignKey('self', null=True, blank=True, related_name='children')
	content = models.TextField(null=True)
	description = models.TextField(null=True)
	keywords = models.TextField(null=True)
	publish_date = models.DateTimeField(null=True)
	status = models.IntegerField()

