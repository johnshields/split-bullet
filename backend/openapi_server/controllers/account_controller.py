import connexion
import six

from openapi_server.models.error import Error  # noqa: E501
from openapi_server.models.inline_object1 import InlineObject1  # noqa: E501
from openapi_server.models.inline_object2 import InlineObject2  # noqa: E501
from openapi_server import util


def login(inline_object1):  # noqa: E501
    """Log in

    Attempts to log a user in # noqa: E501

    :param inline_object1: 
    :type inline_object1: dict | bytes

    :rtype: None
    """
    if connexion.request.is_json:
        inline_object1 = InlineObject1.from_dict(connexion.request.get_json())  # noqa: E501
    return 'do some magic!'


def register(inline_object2):  # noqa: E501
    """Registers User

    Attempts to register a new user # noqa: E501

    :param inline_object2: 
    :type inline_object2: dict | bytes

    :rtype: None
    """
    if connexion.request.is_json:
        inline_object2 = InlineObject2.from_dict(connexion.request.get_json())  # noqa: E501
    return 'do some magic!'
